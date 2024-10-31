using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;
using MyBlog_Clients_MVC.Models;
using Newtonsoft.Json;
using System.Diagnostics;


namespace MyBlog_Clients_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        public HomeController()
        {
            _client = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync("https://localhost:7233/api/Post");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<PostDto>>(strData);
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
