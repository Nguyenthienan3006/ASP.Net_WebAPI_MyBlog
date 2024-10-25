using MyBlog.Interfaces;
using MyBlog.Models;

namespace MyBlog.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyBlog_DBContext _context;

        public CommentRepository(MyBlog_DBContext context)
        {
            _context = context;
        }
        public bool CommentExist(int id)
        {
            return _context.Comments.Any(c => c.Id == id);
        }

        public bool CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            return Save();
        }

        public bool DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            return Save();
        }

        public Comment GetComment(int id)
        {
            return _context.Comments.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Comment> GetComments()
        {
            return _context.Comments.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            return Save();
        }
    }
}
