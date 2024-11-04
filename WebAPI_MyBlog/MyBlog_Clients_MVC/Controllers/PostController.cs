using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;
using Newtonsoft.Json;

namespace MyBlog_Clients_MVC.Controllers
{
    public class PostController : Controller
    {
        private readonly HttpClient _client;
        private readonly INotyfService _notyf;

        public PostController(INotyfService notyf)
        {
            _client = new HttpClient();
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: PostDetail
        [HttpGet]
        public async Task<IActionResult> PostDetail(int postId)
        {

            var token = HttpContext.Session.GetString("AccessToken");
            if(token == null)
            {
                _notyf.Warning("Sign In to see post detail.");
                return RedirectToAction("Index", "Home");
            }

            var request = new HttpRequestMessage(method: HttpMethod.Get, $"https://localhost:7233/api/Post/{postId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            HttpResponseMessage response = await _client.SendAsync(request);


            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewData["NotFound"] = "Not Found !";
                return View();
            }

            string strData = await response.Content.ReadAsStringAsync();
            var postDetail = JsonConvert.DeserializeObject<PostDto>(strData);

            return View(postDetail);
        }

        [HttpGet]
        public async Task<IActionResult> PostByCategory(int categoryId, string categoryName)
        {
            if(categoryName != null)
            {
                HttpContext.Session.SetString("CategoryName", categoryName);
            }

            //Get All posts
            HttpResponseMessage getAllPostResponse = await _client.GetAsync($"https://localhost:7233/api/Categories/{categoryId}/posts");
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
    }
}
