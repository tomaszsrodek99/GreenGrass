using GreenGrassAPI.Dtos;
using GreenGrassAPI.Interfaces;
using GreenGrassUI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using GreenGrassAPI.Models;

namespace GreenGrassUI.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44304/");
        }
        public IActionResult LoginView()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Users/Login", request);
            if (response.IsSuccessStatusCode)
            {
                var responseDto = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();
                string token = responseDto.Token;
                string userId = responseDto.UserId.ToString();
                string nick = responseDto.Nickname;

                HttpContext.Session.SetString("UserId", userId);
                HttpContext.Session.SetString("Nick", nick);

                var jwtCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                };
                Response.Cookies.Append("JWTToken", token, jwtCookie);

                var userIdCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                };
                Response.Cookies.Append("UserId", userId, userIdCookie);

                return RedirectToAction("Index");
            }
            else
            {
                bool userExists = await _httpClient.GetFromJsonAsync<bool>($"api/Users/UserExists?email={request.Email}");
                if (!userExists)
                {
                    ModelState.AddModelError("Email", "Użytkownik o podanym adresie email nie istnieje.");
                    return View("Login", request);
                }
                ModelState.AddModelError("Password", "Podano błedne hasło.");
                return View("Login", request);
            }
        }

        public IActionResult RegisterView()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterRequestDto request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Users/Register", request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("LoginView");
            }
            else
            {

                bool userExists = await _httpClient.GetFromJsonAsync<bool>($"api/Users/UserExists?email={request.Email}");
                bool nickExists = await _httpClient.GetFromJsonAsync<bool>($"api/Users/NickExists?nickname={request.Nickname}");

                if (userExists)
                {
                    ModelState.AddModelError("Email", "Użytkownik o podanym adresie email istnieje.");
                    return View("Register", request);
                }
                else if(nickExists)
                {
                    ModelState.AddModelError("Nickname", "Użytkownik o podanym nicku istnieje.");
                    return View("Register", request);
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = errorMessage;
                return View("Error");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            string userId = Request.Cookies["UserId"];
            ViewBag.UserId = userId;
            return View("Index");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 

            Response.Cookies.Delete("JWTToken");
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("RefreshToken");

            return RedirectToAction("LoginView");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/GetFriends{Request.Cookies["UserId"]}");
                if (response.IsSuccessStatusCode)
                {
                    var friends = await response.Content.ReadFromJsonAsync<IEnumerable<UserFriends>>();
                    return View("Friends", friends);
                }
                else
                {
                    return View("Friends", new List<UserFriends>());
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SearchFriends(User userDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Users/SearchByNick{userDto.Nickname}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    return View("SearchFriends", user);
                }
                else
                {
                    ModelState.AddModelError("searchQuery", "Użytkownik o podanym nicku nie istnieje.");
                    return View();

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}