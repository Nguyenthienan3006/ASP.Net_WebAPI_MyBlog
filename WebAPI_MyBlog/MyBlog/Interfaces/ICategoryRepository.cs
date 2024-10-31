using MyBlog.Models;
using System.Collections.ObjectModel;

namespace MyBlog.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        ICollection<Post> GetPostByCategoryId(int categoryId);
        Category GetCategory(int categoryId);
        string GetPostCategoryName(int postId);
        bool CategoryExist(int categoryId);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
