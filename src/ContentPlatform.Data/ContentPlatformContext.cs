using ContentPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ContentPlatform.Data
{
    public class ContentPlatformContext : DbContext
    {
        private ILoggerFactory MyConsoleLoggerFactory;

        public ContentPlatformContext(DbContextOptions<ContentPlatformContext> options) : base(options)
        { }

        public ContentPlatformContext()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder
                        .AddConsole()
                        .AddFilter
                        (DbLoggerCategory.Database.Command.Name, level => level == LogLevel.Information));

            MyConsoleLoggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyConsoleLoggerFactory)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(
                 "Server=(localdb)\\mssqllocaldb;Database=ContentPlatform;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher>()
                .HasMany(p => p.Authors)
                .WithOne(a => a.Publisher)
                .HasForeignKey(a => a.PublisherId);
            //.HasConstraintName("FK_Author_Publisher_PublisherId")

            modelBuilder.Entity<Publisher>()
                .HasMany(p => p.Blogs)
                .WithOne(b => b.Publisher)
                .HasForeignKey(b => b.PublisherId);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts)
                .WithOne(p => p.Blog)
                .HasForeignKey(p => p.BlogId);

            modelBuilder.Entity<Post>()
                .ToTable("BlogPosts")
                .HasKey(p => p.PostId);

            modelBuilder.Entity<Publisher>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Publisher>()
                .Property(p => p.MainWebsite)
                .IsRequired();

            modelBuilder.Entity<Author>()
                .Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(60);
            modelBuilder.Entity<Author>()
                .Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Author>()
                .Property(a => a.Email)
                .IsRequired();

            modelBuilder.Entity<Blog>()
                .Ignore(b => b.TakeDownTime);
            modelBuilder.Ignore<BlogMetadata>();

            SeedData(modelBuilder);

        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    AuthorId = 1,
                    Email = "a@a.a",
                    FirstName = "John",
                    LastName = "Snow",
                    PublisherId = 1
                },
                new Author
                {
                    AuthorId = 2,
                    Email = "b@b.b",
                    FirstName = "Arya",
                    LastName = "Stark",
                    PublisherId = 1
                },
                new Author
                {
                    AuthorId = 3,
                    Email = "c@c.c",
                    FirstName = "Margaery",
                    LastName = "Tyrell",
                    PublisherId = 1
                });
        }
    }
}