using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenGrassAPI.Repositories;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Models;
using GreenGrassAPI.Interfaces;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace GreenGrassAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _repository = userRepository;
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _repository.GetAllAsync();
                if (users == null)
                {
                    return BadRequest("Nie znaleziono użytkowników");
                }
                var records = _mapper.Map<List<UserDto>>(users);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUser{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _repository.GetAsync(id);
                if (user == null)
                {
                    return BadRequest("Nie znaleziono użytkownika.");
                }
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutUser(UserDto userDto)
        {
            try
            {
                var user = await _repository.GetAsync(userDto.Id);
                if (user == null)
                {
                    return BadRequest("Nie znaleziono użytkownika o podanym ID.");
                }
                var users = await _repository.GetAllAsync();
                var duplicate = users.Where(x => x.Email == userDto.Email && x.Id != userDto.Id);
                if (duplicate.Any())
                {
                    return BadRequest("Użytkownik o podanym adresie email już istnieje.");
                }
                _mapper.Map(userDto, user);

                await _repository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _repository.GetAsync(id);
                if (user == null)
                {
                    return NotFound("Nie znaleziono użytkownika o podanym adresie email.");
                }
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestDto request)
        {
            try
            {
                var user = await _repository.Register(request);
                if (user == null)
                    return BadRequest("Użytkownik o podanym adresie email już istnieje.");
                else
                    return Ok("Poprawnie zarejestrowano użytkownika!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            try
            {
                var user = await _repository.GetUserByLogin(request.Email);
                if (user == null)
                {
                    return BadRequest();
                }

                var passwordValid = _repository.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
                if (!passwordValid)
                {
                    return BadRequest("Błędne hasło");
                }

                string token = _repository.GenerateToken(user);

                var refreshToken = GenerateRefreshToken();

                SetRefreshToken(refreshToken, user.Id);

                var responseDto = new UserLoginResponseDto() { Token = token, UserId = user.Id };
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var userDto = GetUser(int.Parse(Request.Cookies["UserId"]));
            var user = _mapper.Map<User>(userDto);

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Nieprawidłowy token.");
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token wygasł.");
            }

            string token = _repository.GenerateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user.Id);

            var responseDto = new UserLoginResponseDto() { Token = token, UserId = user.Id };
            return Ok(responseDto);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMinutes(60),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        private async void SetRefreshToken(RefreshToken refreshToken, int id)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.Expires
            };
            Response.Cookies.Append("RefreshToken", refreshToken.Token, cookieOptions);

            var user = await _repository.GetAsync(id);

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;

            _repository.UpdateAsync(user);
        }

        [HttpGet("UserExists")]
        public async Task<ActionResult<bool>> CheckIfUserExists(string email)
        {
            var user = await _repository.GetUserByLogin(email);
            return user != null;
        }
    }
}
