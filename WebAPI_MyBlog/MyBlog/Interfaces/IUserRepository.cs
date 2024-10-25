using MyBlog.Models;

namespace MyBlog.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUserByName(string username);
        ICollection<Post> GetPostByUserId(int userId);
        bool UserExist(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
