using System;
using System.Collections.Generic;

namespace MyBlog.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!; // Lưu trữ hash của mật khẩu
        public byte[] PasswordSalt { get; set; } = null!; // Lưu trữ salt dùng để mã hóa
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
