using ContentPlatform.Data.Converters;
using ContentPlatform.Data.EntityConfigurations;
using ContentPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
                        .AddFilter(DbLoggerCategory.Database.Command.Name, level => level == LogLevel.Information)
                        .AddFilter(DbLoggerCategory.ChangeTracking.Name, level => level == LogLevel.Debug));

            MyConsoleLoggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();

            this.ChangeTracker.StateChanged += StateChanged;
            this.ChangeTracker.Tracked += Tracked;
        }

        private void Tracked(object sender, EntityTrackedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void StateChanged(object sender, EntityStateChangedEventArgs e)
        {
            
            //throw new NotImplementedException();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbQuery<BlogStatistics> BlogStatistics { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder
                .UseLoggerFactory(MyConsoleLoggerFactory)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(
                 "Server=(localdb)\\mssqllocaldb;Database=ContentPlatform;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddRelationships(modelBuilder);

            AddColumnConstraints(modelBuilder);

            AddExclusions(modelBuilder);

            AddDefaultValues(modelBuilder);

            AddConversions(modelBuilder);

            AddShadowProperties(modelBuilder);

            AddGlobalQueryFilters(modelBuilder);

            AddQueryTypes(modelBuilder);

            SeedData(modelBuilder);

            modelBuilder.ApplyConfiguration(new PublisherEntityTypeConfiguration());

        }

        private void AddQueryTypes(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Query<BlogStatistics>().ToView("View_BlogPostCount")
                .Property(bs => bs.PostCount).HasColumnName("Count");

            // DEFINING QUERY with LINQ
            //modelBuilder.Query<BlogStatistics>().ToQuery(
            //    () => Blogs.Include(b => b.Posts).Select(
            //        b => new BlogStatistics
            //        {
            //            PostCount = b.Posts.Count,
            //            Title = b.Title,
            //            Url = b.Url
            //        }
            //    ));


            //DEFINING QUERY with RAW SQL and Stored Procedure
            //modelBuilder.Query<BlogStatistics>().ToQuery(
            //    () => Query<BlogStatistics>().FromSql(@"EXEC SomeStoredProc {0}",1)
            //    );

            // DEFINING QUERY with RAW SQL
            //modelBuilder.Query<BlogStatistics>().ToQuery(
            //    () => Query<BlogStatistics>().FromSql(
            //        @"
            //        SELECT b.Title, b.Url,  COUNT(p.PostId) as PostCount
            //        FROM Blogs b
            //        JOIN BlogPosts p on p.BlogId = b.BlogId
            //        GROUP BY b.Title, b.Url
            //    ")
            //);
        }

        private void AddGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasQueryFilter(p => !string.IsNullOrEmpty(p.Content));
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var timestamp = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified)
                && !e.Metadata.IsOwned()))
            {
                entry.Property("LastModified").CurrentValue = timestamp;

                if (entry.Entity is Post)
                {
                    //if (entry.Reference("Metadata").CurrentValue == null)
                    //{
                    //    entry.Reference("Metadata").CurrentValue = PostMetadata.Empty();
                    //}
                    entry.Reference("Metadata").TargetEntry.State = entry.State;
                }

                //if (entry.State == EntityState.Added)
                //{
                //    entry.Property("Created").CurrentValue = timestamp;
                //}
            }
            return base.SaveChanges();
        }

        private void AddShadowProperties(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!entityType.IsOwned() && !entityType.IsQueryType)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("Created").HasDefaultValueSql("GETDATE()");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("LastModified");
                }
            }
        }

        private static void AddRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts)
                .WithOne(p => p.Blog)
                .HasForeignKey(p => p.BlogId);

            modelBuilder.Entity<Contribution>()
                .ToTable("Contributions")
                .HasKey(c => new { c.AuthorId, c.PostId });
            modelBuilder.Entity<Contribution>()
                .HasOne(c => c.Author)
                .WithMany(a => a.Contributions)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Contribution>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Contributions)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>().OwnsOne(p => p.Metadata,
                pm =>
                {
                    pm.Property(x => x.Keywords).HasColumnName(nameof(PostMetadata.Keywords));
                });
        }

        private static void AddColumnConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                            .ToTable("BlogPosts")
                            .HasKey(p => p.PostId);

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
        }

        private static void AddExclusions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                            .Ignore(b => b.TakeDownTime);

            modelBuilder.Ignore<BlogMetadata>();
        }

        private static void AddDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(p => p.Version)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("NEWID()");
        }

        private static void AddConversions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(p => p.TitleBackgroundColor)
                .HasConversion(
                    to => to.Name,
                    from => Color.FromName(from)
                );
            //modelBuilder.Entity<Post>()
            //    .Property(p => p.TitleBackgroundColor)
            //    .HasConversion(ColorConverter);

            //modelBuilder.Entity<Post>()
            //    .Property(p => p.TitleBackgroundColor)
            //    .HasConversion(new ColorToStringValueConverter());

            modelBuilder.Entity<Blog>()
                .Property(b => b.BlogType)
                .HasConversion(new EnumToStringConverter<BlogType>());
        }

        private ValueConverter<Color, string> ColorConverter
            = new ValueConverter<Color, string>(c => c.Name, s => Color.FromName(s));

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