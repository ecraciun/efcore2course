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
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    AuthorId = 1, Email = "a@a.a", FirstName = "John", LastName = "Snow"
                },
                new Author
                {
                    AuthorId = 2, Email = "b@b.b", FirstName = "Arya", LastName = "Stark"
                },
                new Author
                {
                    AuthorId = 3, Email = "c@c.c", FirstName = "Margaery", LastName = "Tyrell"
                }
            );

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher
                {
                    MainWebsite = "http://a.a.a",
                    Name = "G.R.R.M.",
                    PublisherId = 1
                },
                new Publisher
                {
                    MainWebsite = "http://contoso.com",
                    Name = "Contoso",
                    PublisherId = 2
                }
            );


        }
    }
}