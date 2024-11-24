using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MyBlog_Clients_MVC.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

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

        //=============================================================== Posts ================================================================
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
        public async Task<IActionResult> CreatePost(string Title, string Content, int CategoryId)
        {
            // Lấy token từ Session
            var token = HttpContext.Session.GetString("AccessToken");

            //kiểm tra xem đã log in chưa
            if (token == null)
            {
                _notyf.Warning("Sign In to Add Post");
                return RedirectToAction("Login", "Auth");
            }

            // Kiểm tra nếu thông tin đầu vào không đầy đủ
            if (Title == null || Content == null)
            {
                _notyf.Warning("Fill Required Information!");
                return RedirectToAction("CreatePost", "Admin");
            }

            // Tạo đối tượng DTO chứa dữ liệu bài đăng mới
            NewPostDto newPost = new NewPostDto()
            {
                Title = Title,
                Content = Content,
            };


            // Cấu hình yêu cầu HTTP POST tới API
            var request = new HttpRequestMessage(method: HttpMethod.Post, $"https://localhost:7233/api/Post?categoryId={CategoryId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Chuyển đổi dữ liệu bài đăng sang JSON
            var postData = new StringContent(JsonConvert.SerializeObject(newPost), Encoding.UTF8, "application/json");
            request.Content = postData;

            try
            {
                // Gửi yêu cầu đến API
                HttpResponseMessage response = await _client.SendAsync(request);

                // Xử lý phản hồi từ API
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _notyf.Success("Post created successfully!");
                    return RedirectToAction("Index", "Admin");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _notyf.Warning("Category not found!");
                    return RedirectToAction("CreatePost", "Admin");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _notyf.Warning("Login To Create Post!");
                    return RedirectToAction("Login", "Auth");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    _notyf.Warning("Post tittle already exists!");
                    return RedirectToAction("CreatePost", "Admin");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    _notyf.Warning("Invalid input data!");
                    return RedirectToAction("CreatePost", "Admin");
                }
                else
                {
                    _notyf.Error("Something went wrong!");
                    return RedirectToAction("CreatePost", "Admin");
                }


            }
            catch (Exception ex)
            {

                _notyf.Error($"{ex.Message}");
                return View();
            }
        }

        public async Task<IActionResult> DeletePost(int postId)
        {
            //Lấy ra token từ session
            var token = HttpContext.Session.GetString("AccessToken");

            //kiểm tra xem đã log in chưa
            if (token == null)
            {
                _notyf.Warning("Sign In to Delete Post");
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

    }
}
