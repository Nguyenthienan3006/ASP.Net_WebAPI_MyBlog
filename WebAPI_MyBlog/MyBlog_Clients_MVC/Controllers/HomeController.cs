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
            //Get All posts
            HttpResponseMessage getAllPostResponse = await _client.GetAsync("https://localhost:7233/api/Post");           
            if (getAllPostResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData = await getAllPostResponse.Content.ReadAsStringAsync();
            var postsList = JsonConvert.DeserializeObject<List<PostDto>>(strData);


            //Get newest posts
            HttpResponseMessage getNewestPosts = await _client.GetAsync("https://localhost:7233/api/Post/newestPosts");
            if (getNewestPosts.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData2 = await getNewestPosts.Content.ReadAsStringAsync();
            var newestPosts = JsonConvert.DeserializeObject<List<PostDto>>(strData2);


            //Get categoriesList
            HttpResponseMessage getCategoriesResponse = await _client.GetAsync("https://localhost:7233/api/Categories");
            if (getCategoriesResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }
            string strData3 = await getCategoriesResponse.Content.ReadAsStringAsync();
            var categoriesList = JsonConvert.DeserializeObject<List<CategoryDto>>(strData3);



            ViewBag.PostsList = postsList;
            ViewBag.NewestPosts = newestPosts;  
            ViewBag.CategoriesList = categoriesList;

            return View();
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
