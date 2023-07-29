using GreenGrassAPI.Dtos;
using GreenGrassAPI.Interfaces;
using GreenGrassUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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

                var jwtCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(90)
                };
                Response.Cookies.Append("JWTToken", token, jwtCookie);

                var userIdCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(90)
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
                if (userExists)
                {
                    ModelState.AddModelError("Email", "Użytkownik o podanym adresie email istnieje.");
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
            string jwtToken = Request.Cookies["JWTToken"];
            string userId = Request.Cookies["UserId"];
            ViewBag.JWTToken = jwtToken;
            ViewBag.UserId = userId;
            return View("Index");
        }
    }
}