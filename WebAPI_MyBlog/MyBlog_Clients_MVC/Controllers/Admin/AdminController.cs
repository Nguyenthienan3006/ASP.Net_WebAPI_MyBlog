using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;
using Newtonsoft.Json;

namespace MyBlog_Clients_MVC.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly HttpClient _client;
        private readonly INotyfService _notyf;
        public AdminController(INotyfService notyf)
        {
            _client = new HttpClient();
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {

            //Get categoriesList
            HttpResponseMessage getCategoriesResponse = await _client.GetAsync("https://localhost:7233/api/Categories");
            if (getCategoriesResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData3 = await getCategoriesResponse.Content.ReadAsStringAsync();
            var categoriesList = JsonConvert.DeserializeObject<List<CategoryDto>>(strData3);
            ViewBag.CategoriesList = categoriesList;

            //lưu vào session để sử dụng ở các trang khác nữa
            var jsonString = JsonConvert.SerializeObject(categoriesList);
            HttpContext.Session.SetString("CategoriesList", jsonString);
            

            //Get postList
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
