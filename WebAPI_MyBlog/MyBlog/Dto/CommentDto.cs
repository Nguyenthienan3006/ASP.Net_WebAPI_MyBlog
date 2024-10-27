namespace MyBlog.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }

    }
}
