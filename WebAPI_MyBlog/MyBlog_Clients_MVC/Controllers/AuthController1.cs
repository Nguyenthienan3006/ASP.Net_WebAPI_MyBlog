using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;

namespace MyBlog_Clients_MVC.Controllers
{
    public class AuthController1 : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController1(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://yourapi.com/api/auth/register", registerDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                return View(registerDto);
            }

            // Optional: Redirect to login page after successful registration
            return RedirectToAction("Login");
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //// POST: Login
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginDto loginDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(loginDto);
        //    }

        //    var client = _httpClientFactory.CreateClient();
        //    var response = await client.PostAsJsonAsync("https://yourapi.com/api/auth/login", loginDto);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid username or password.");
        //        return View(loginDto);
        //    }

        //    var tokenData = await response.Content.ReadFromJsonAsync<TokenResponse>();
        //    HttpContext.Session.SetString("AccessToken", tokenData.AccessToken);

        //    // Optional: Redirect to a protected page after successful login
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
