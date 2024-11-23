using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;
using Newtonsoft.Json;

namespace MyBlog_Clients_MVC.Controllers.Admin.Post
{
    public class AdminPostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            //lấy ra category list
            string CategoryListJson = HttpContext.Session.GetString("CategoryListJson");


            if (!string.IsNullOrEmpty(CategoryListJson))
            {
                // Giải mã JSON thành danh sách
                List<CategoryDto> CategoriesList = JsonConvert.DeserializeObject<List<CategoryDto>>(CategoryListJson);

                ViewBag.CategoriesList = CategoriesList;
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(string Title, )
        {
            return View();
        }
    }
}
