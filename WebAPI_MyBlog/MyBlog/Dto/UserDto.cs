namespace MyBlog.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
