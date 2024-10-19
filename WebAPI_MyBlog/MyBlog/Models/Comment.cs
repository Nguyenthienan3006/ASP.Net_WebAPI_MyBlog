using System;
using System.Collections.Generic;

namespace MyBlog.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
        public int? UsersId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User? Users { get; set; }
    }
}
