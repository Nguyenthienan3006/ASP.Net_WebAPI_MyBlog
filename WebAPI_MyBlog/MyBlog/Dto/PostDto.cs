namespace MyBlog.Dto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Author { get; set; }
        public int CommentNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PostCategory { get; set; }
    }
}
