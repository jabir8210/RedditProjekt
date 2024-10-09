using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Data
{
    public class PostContext : DbContext
    {
        public DbSet<Post> Post { get; set; }

        public DbSet<User> User { get; set; }
        public string DbPath { get; }

        public PostContext(DbContextOptions<PostContext> options)
        {
            DbPath = "bin/Post.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Post");
        }
    }
    
}
