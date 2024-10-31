using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;
using MyBlog_Clients_MVC.Models;

namespace MyBlog_Clients_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _client;
        private readonly INotyfService _notyf;

        public AuthController(INotyfService notyf)
        {
            _client = new HttpClient();
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View();
        }


        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var response = await _client.PostAsJsonAsync("https://localhost:7233/api/Auth/register", registerDto);

            if (!response.IsSuccessStatusCode)
            {
                // Đọc message từ API response
                var errorMessage = await response.Content.ReadAsStringAsync();
            
                _notyf.Warning(errorMessage);
                return View(registerDto);
            }

            // Optional: Redirect to login page after successful registration
            _notyf.Success("Register Succesfully");
            return RedirectToAction("Login");
        }

        // GET: Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var response = await _client.PostAsJsonAsync("https://localhost:7233/api/Auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                // Đọc message từ API response
                var errorMessage = await response.Content.ReadAsStringAsync();

                _notyf.Warning(errorMessage);
                return View(loginDto);  
            }

            var tokenData = await response.Content.ReadFromJsonAsync<TokenResponse>();
            HttpContext.Session.SetString("AccessToken", tokenData.AccessToken);
            HttpContext.Session.SetString("UserName", tokenData.UserName);
            if(tokenData.UserName == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            //Optional: Redirect to a protected page after successful login
            _notyf.Success("Login Succesfully");
            return RedirectToAction("Index", "Home");
        }
    }
}
