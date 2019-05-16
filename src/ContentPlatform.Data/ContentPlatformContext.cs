using ContentPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContentPlatform.Data
{
    public class ContentPlatformContext : DbContext
    {
        public ContentPlatformContext(DbContextOptions<ContentPlatformContext> options) : base(options)
        { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}