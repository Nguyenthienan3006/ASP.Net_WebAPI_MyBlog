using MyBlog.Interfaces;
using MyBlog.Models;

namespace MyBlog.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyBlog_DBContext _context;

        public UserRepository(MyBlog_DBContext context)
        {
            _context = context;
        }

        public ICollection<Post> GetPostByUserId(int userId)
        {
            return _context.Posts.Where(p => p.UsersId == userId).ToList();
        }
        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        public User GetUser(int id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool UserExist(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        public User GetUserByName(string username)
        {
            return _context.Users.Where(u => u.UserName.Trim().ToLower() == username.TrimEnd().ToLower()).FirstOrDefault();
        }
    }
}
