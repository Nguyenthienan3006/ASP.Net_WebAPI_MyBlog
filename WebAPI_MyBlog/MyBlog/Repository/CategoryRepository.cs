using Microsoft.EntityFrameworkCore;
using MyBlog.Interfaces;
using MyBlog.Models;

namespace MyBlog.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyBlog_DBContext _context;

        public CategoryRepository(MyBlog_DBContext context)
        {
            _context = context;
        }

        public bool CategoryExist(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public ICollection<Post> GetPostByCategory(int categoryId)
        {
            return _context.PostCategories.Where(p => p.CategoryId == categoryId).Select(p => p.Post).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return Save();
        }
    }
}
