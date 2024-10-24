using System;
using System.Collections.Generic;

namespace MyBlog.Models
{
    public partial class Category
    {
        public Category()
        {
            PostCategories = new HashSet<PostCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<PostCategory> PostCategories { get; set; }
    }
}
