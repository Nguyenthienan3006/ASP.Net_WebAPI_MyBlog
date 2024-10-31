using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;
using Newtonsoft.Json;

namespace MyBlog_Clients_MVC.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly HttpClient _client;
        public AdminController()
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
    }
}
