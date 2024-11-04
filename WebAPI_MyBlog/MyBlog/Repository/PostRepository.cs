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
        public bool CreatePost(Post post, int categoryId, int userId)
        {


            Post post1 = new Post
            {
                Title = post.Title,
                Content = post.Content,
                CreatedAt = DateTime.Now,
                UsersId = userId
            };

            _context.Posts.Add(post1);

            _context.SaveChanges();

            //Add post category in to postCategory table
            PostCategory postCategory = new PostCategory
            {
                PostId = post1.Id,
                CategoryId = categoryId,
                CreatedAt= DateTime.Now
            };

            _context.PostCategories.Add(postCategory);

            return Save();
        }

        public bool DeletePost(Post post)
        {
            var postToDelete = _context.Comments.Where(comment => comment.PostId == post.Id).ToList();

            foreach (var comment in postToDelete)
            {
                _context.Comments.Remove(comment);
            }

            _context.Posts.Remove(post);
            return Save();
        }

        public ICollection<Comment> GetCommentOfPost(int postId)
        {
            return _context.Comments.Where(c => c.PostId == postId).ToList();
        }

        public ICollection<Post> GetNewestPosts()
        {
            return _context.Posts.OrderByDescending(p => p.CreatedAt).Take(3).ToList();
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
