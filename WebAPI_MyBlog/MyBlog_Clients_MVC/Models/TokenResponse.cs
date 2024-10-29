namespace MyBlog_Clients_MVC.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        // Thêm các thuộc tính khác nếu cần, ví dụ như RefreshToken, ExpiresIn, v.v.
    }
}
