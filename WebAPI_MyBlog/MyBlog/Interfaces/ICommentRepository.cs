using MyBlog.Models;
using System.Diagnostics.Eventing.Reader;

namespace MyBlog.Interfaces
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetComments();
        Comment GetComment(int id);
        bool CreateComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(Comment comment);
        bool CommentExist(int id);
        bool Save();
    }
}
