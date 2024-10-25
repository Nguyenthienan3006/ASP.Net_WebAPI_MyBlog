using MyBlog.Interfaces;
using MyBlog.Models;

namespace MyBlog.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly MyBlog_DBContext _context;

        public PostRepository(MyBlog_DBContext context)
        {
            _context = context;
        }
        public bool CreatePost(Post post)
        {
            _context.Posts.Add(post);
            return Save();
        }

        public bool DeletePost(Post post)
        {
            _context.Posts.Remove(post);
            return Save();
        }

        public ICollection<Comment> GetCommentOfPost(int postId)
        {
            return _context.Comments.Where(c => c.PostId == postId).ToList();
        }

        public Post GetPost(int postId)
        {
            return _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
        }




        public ICollection<Post> GetPosts()
        {
            return _context.Posts.ToList();
        }

        public bool PostExist(int postId)
        {
            return _context.Posts.Any(p => p.Id == postId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePost(Post post)
        {
            _context.Posts.Update(post);
            return Save();
        }
    }
}
