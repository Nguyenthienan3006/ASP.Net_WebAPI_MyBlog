namespace MyBlog.Dto
{
    public class UserToCreateDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
