using System;
using System.Collections.Generic;

namespace MyBlog.Models
{
    public partial class PostCategory
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
    }
}
