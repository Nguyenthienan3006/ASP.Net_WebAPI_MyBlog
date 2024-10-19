using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyBlog.Models
{
    public partial class MyBlog_DBContext : DbContext
    {
        public MyBlog_DBContext()
        {
        }

        public MyBlog_DBContext(DbContextOptions<MyBlog_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server = (local); database = MyBlog_DB; uid = sa; pwd = 123456; Trusted_Connection=True; TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.UsersId).HasColumnName("Users_Id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__Post_I__36B12243");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UsersId)
                    .HasConstraintName("FK__Comments__Users___37A5467C");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UsersId).HasColumnName("Users_Id");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UsersId)
                    .HasConstraintName("FK__Posts__Users_Id__267ABA7A");

                entity.HasMany(d => d.Categories)
                    .WithMany(p => p.Posts)
                    .UsingEntity<Dictionary<string, object>>(
                        "PostCategory",
                        l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__PostCateg__Categ__3D5E1FD2"),
                        r => r.HasOne<Post>().WithMany().HasForeignKey("PostId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__PostCateg__Post___3C69FB99"),
                        j =>
                        {
                            j.HasKey("PostId", "CategoryId").HasName("PK__PostCate__AEAECF7B53643F80");

                            j.ToTable("PostCategory");

                            j.IndexerProperty<int>("PostId").HasColumnName("Post_Id");

                            j.IndexerProperty<int>("CategoryId").HasColumnName("Category_Id");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.UserName).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
