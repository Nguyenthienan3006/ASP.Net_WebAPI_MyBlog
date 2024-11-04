using MyBlog.Models;

namespace MyBlog.Interfaces
{
    public interface IPostRepository
    {
        ICollection<Post> GetPosts();
        Post GetPost(int postId);
        ICollection<Post> GetNewestPosts();
        ICollection<Comment> GetCommentOfPost(int postId);
        bool CreatePost(Post post, int categoryId, int userId);
        bool UpdatePost(Post post);
        bool DeletePost(Post post);
        bool PostExist(int postId);
        bool Save();
    }
}
