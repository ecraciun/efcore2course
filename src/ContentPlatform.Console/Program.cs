using ContentPlatform.Data;
using ContentPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Console = System.Console;

namespace ContentPlatform.Console
{
    class Program
    {
        private static ContentPlatformContext _ctx;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            using (_ctx = new ContentPlatformContext())
            {
                _ctx.Database.Migrate();
                //RunSingleExamples();
                //AddEntityWithGeneratedValue();
                //RunManyToManyExamples();
                //RunOneToOneExamples();
                //RunShadowPropertiesExamples();
                //RunOwnedPropertiesExamples();
                RunGlobalQueryFilterExamples();
            }
        }

        private static void RunGlobalQueryFilterExamples()
        {
            CreatePostWithoutContent();
            GetEmptyContentPost();
        }

        private static void GetEmptyContentPost()
        {
            var allPosts = _ctx.Posts.ToList();
            var hasEmptyContent = allPosts.Any(p => string.IsNullOrEmpty(p.Content));

            var allPosts2 = _ctx.Posts.IgnoreQueryFilters().ToList();
            var hasEmptyContent2 = allPosts2.Any(p => string.IsNullOrEmpty(p.Content));
        }

        #region Global query filter

        private static void CreatePostWithoutContent()
        {
            var post = new Post
            {
                Title = "abc123",
                BlogId = 3,
            };
            _ctx.Posts.Add(post);
            _ctx.SaveChanges();
        }

        #endregion


        #region Owned properties

        private static void RunOwnedPropertiesExamples()
        {
            //CreatePostWithMetadata();
            //ReplaceMetadata();
            //CreatePostWithoutMetadata();
        }

        private static void CreatePostWithoutMetadata()
        {
            var post = new Post
            {
                Title = "123",
                Content = "etc",
                BlogId = 3,
            };
            _ctx.Posts.Add(post);
            _ctx.SaveChanges();
        }

        private static void ReplaceMetadata()
        {
            var post = _ctx.Posts.FirstOrDefault(p => p.Metadata.Keywords == "a");
            _ctx.Entry(post).Reference(p => p.Metadata).TargetEntry.State = EntityState.Detached;
            post.Metadata = PostMetadata.Create("b", "b");
            _ctx.Posts.Update(post);
            _ctx.SaveChanges();
        }

        private static void CreatePostWithMetadata()
        {
            var post = new Post
            {
                Title = "abc",
                Content = "etc",
                BlogId = 3,
                Metadata = PostMetadata.Create("a", "a")
            };
            _ctx.Posts.Add(post);
            _ctx.SaveChanges();
        }

        #endregion Owned properties

        #region Shadow properties

        private static void RunShadowPropertiesExamples()
        {
            //CreateLocation();
            //RetrieveLocationsCreatedInPastWeek();
            CreateThenEditBlogWithPost();
        }

        private static void RetrieveLocationsCreatedInPastWeek()
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            //var newLocations = _ctx.Locations
            //                          .Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
            //                          .ToList();
            var locationsCreated = _ctx.Locations
                                        .Where(l => EF.Property<DateTime>(l, "Created") >= oneWeekAgo)
                                        .Select(l => new { l.LocationId, l.Address, Created = EF.Property<DateTime>(l, "Created") })
                                        .ToList();
        }

        private static void CreateLocation()
        {
            var location = new Location { Address = "Somewhere over the rainbow" };
            _ctx.Locations.Add(location);
            //_ctx.Entry(location).Property("Created").CurrentValue = DateTime.Now;
            //_ctx.Entry(location).Property("LastModified").CurrentValue = DateTime.Now;
            _ctx.SaveChanges();
        }

        private static void CreateThenEditBlogWithPost()
        {
            var blog = new Blog
            {
                BlogType = BlogType.Gaming,
                PublisherId = 1,
                Title = "Level",
                Url = "nivelul2.ro"
            };
            var post = new Post { Content = "lorem ipsum", Title = "New game out!" };
            blog.Posts.Add(post);
            _ctx.Blogs.Add(blog);
            _ctx.SaveChanges();
            post.Content += " Here is where to buy it.";
            _ctx.SaveChanges();
        }

        #endregion Shadow properties

        #region Single

        private static void RunSingleExamples()
        {
            //QueryAndUpdateLocation_Disconnected();
            //MultipleDatabaseOperations();
            //RetrieveAndUpdateMultipleLocations();
            //RetrieveAndUpdateLocation();
            //MoreQueries();
            //DeleteWhileNotTracked();
            //DeleteMany();
            //DeleteWhileTracked();
            //DeleteUsingId(5);
        }


        private static void DeleteUsingId(int locationId)
        {
            var location = _ctx.Locations.Find(locationId);
            _ctx.Remove(location);
            _ctx.SaveChanges();
            //alternate: call a stored procedure!
            //_ctx.Database.ExecuteSqlCommand("exec DeleteById {0}", locationId);
        }


        private static void DeleteWhileTracked()
        {
            var location = _ctx.Locations.FirstOrDefault(s => s.Address == "London2");
            _ctx.Locations.Remove(location);
            //alternates:
            // _ctx.Remove(location);
            // _ctx.Entry(location).State=EntityState.Deleted;
            // _ctx.Locations.Remove(_ctx.Locations.Find(1));
            _ctx.SaveChanges();
        }

        private static void DeleteMany()
        {
            var locations = _ctx.Locations.Where(s => s.Address.Contains("AAA"));
            _ctx.Locations.RemoveRange(locations);
            //alternate: _ctx.RemoveRange(locations);
            _ctx.SaveChanges();
        }

        private static void DeleteWhileNotTracked()
        {
            var location = _ctx.Locations.FirstOrDefault(s => s.Address == "AAA2");
            using (var contextNewAppInstance = new ContentPlatformContext())
            {
                contextNewAppInstance.Locations.Remove(location);
                //contextNewAppInstance.Entry(location).State=EntityState.Deleted;
                contextNewAppInstance.SaveChanges();
            }
        }

        private static void MoreQueries()
        {
            var locations_NonParameterizedQuery = _ctx.Locations.Where(l => l.Address == "Chicago2").ToList();
            var name = "AAA2";
            var locations_ParameterizedQuery = _ctx.Locations.Where(l => l.Address == name).ToList();
            var location_Object = _ctx.Locations.FirstOrDefault(l => l.Address == name);
            var locations_ObjectFindByKeyValue = _ctx.Locations.Find(2);
            var lastLocation = _ctx.Locations.OrderBy(s => s.LocationId).LastOrDefault(l => l.Address == name);
            var locationsA = _ctx.Locations.Where(s => EF.Functions.Like(s.Address, "A%")).ToList();

        }

        private static void RetrieveAndUpdateLocation()
        {
            var location = _ctx.Locations.FirstOrDefault();
            location.Address += "3";
            _ctx.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleLocations()
        {
            var locations = _ctx.Locations.ToList();
            locations.ForEach(s => s.Address += "2");
            _ctx.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var location = _ctx.Locations.FirstOrDefault();
            location.Address += "1";
            _ctx.Locations.Add(new Location { Address = "AAA" });
            _ctx.SaveChanges();
        }


        private static void QueryAndUpdateLocation_Disconnected()
        {
            var location = _ctx.Locations.FirstOrDefault();
            location.Address = "ABC";
            using (var newContextInstance = new ContentPlatformContext())
            {
                newContextInstance.Locations.Update(location);
                newContextInstance.SaveChanges();
            }
        }

        #endregion Single

        #region One to one

        private static void RunOneToOneExamples()
        {
            //AddNewPublisherWithLocation();
            //AddLocationUsingPublisherId();
            //AddLocationToExistingPublisher();
            //EditALocation();
            //ReplaceALocation();
            //ReplaceALocationNotTracked();
            //ReplaceLocationNotInMemory();
        }

        private static void ReplaceLocationNotInMemory()
        {
            var publisher = _ctx.Publishers.FirstOrDefault(s => s.MainOffice != null);
            publisher.MainOffice = new Location { Address = "Berlin" };
            //this will fail...EF Core tries to insert a duplicate PublisherId FK
            _ctx.SaveChanges();
        }

        private static void ReplaceALocationNotTracked()
        {
            Publisher publisher;
            using (var separateOperation = new ContentPlatformContext())
            {
                publisher = _ctx.Publishers.Include(p => p.MainOffice)
                                  .FirstOrDefault(p => p.PublisherId == 1);
            }
            publisher.MainOffice = new Location { Address = "Bucharest" };
            _ctx.Publishers.Attach(publisher);
            
            _ctx.SaveChanges();
        }

        private static void ReplaceALocation()
        {
            var publisher = _ctx.Publishers.Include(p => p.MainOffice)
                                  .FirstOrDefault(p => p.PublisherId == 1);
            publisher.MainOffice = new Location { Address = "Chicago" };
            _ctx.SaveChanges();
        }

        private static void EditALocation()
        {
            var publisher = _ctx.Publishers.Include(p => p.MainOffice)
                                  .FirstOrDefault(p => p.PublisherId == 1);
            publisher.MainOffice.Address = "London";
            _ctx.SaveChanges();
        }

        private static void AddLocationToExistingPublisher()
        {
            Publisher publisher;
            using (var separateOperation = new ContentPlatformContext())
            {
                publisher = _ctx.Publishers.Find(2);
            }
            publisher.MainOffice = new Location { Address = "Paris" };
            _ctx.Publishers.Attach(publisher);
            _ctx.SaveChanges();
        }

        private static void AddLocationUsingPublisherId()
        {
            //Note: PublisherId 1 does not have a location yet!
            var identity = new Location { PublisherId = 1, Address = "NYC" };
            _ctx.Add(identity);
            _ctx.SaveChanges();
        }

        private static void AddNewPublisherWithLocation()
        {
            var publisher = new Publisher { Name = "EA", MainWebsite ="a" };
            publisher.MainOffice = new Location { Address = "Seattle" };
            _ctx.Publishers.Add(publisher);
            _ctx.SaveChanges();
        }


        #endregion One to one

        #region Many to many

        private static void RunManyToManyExamples()
        {
            //PrePopulateAfterManyToMany();
            //JoinAuthorsAndPosts();
            //AddPostContribution();
            //AddPostContributionUntracked();
            //AddNewAuthorViaDisconnectedPostObject();
            //GetAuthorWithPosts();
            //GetAuthorsForPostInMemory();
            //RemoveJoinBetweenAuthorAndPostSimple();
            //RemovePostFromAuthor();
            //RemovePostFromAuthorWhenDisconnected();
        }

        private static void RemovePostFromAuthorWhenDisconnected()
        {
            Author author;
            using (var separateOperation = new ContentPlatformContext())
            {
                author = separateOperation.Authors
                    .Include(s => s.Contributions)
                    .ThenInclude(c => c.Post)
                    .SingleOrDefault(s => s.AuthorId == 1);
            }
            var cToRemove = author.Contributions.SingleOrDefault(sb => sb.PostId == 5);
            _ctx.Attach(author);
            author.Contributions.Remove(cToRemove);
            _ctx.ChangeTracker.DetectChanges();
            //_ctx.Remove(cToRemove);
            _ctx.SaveChanges();
        }

        private static void RemovePostFromAuthor()
        {
            var author = _ctx.Authors.Include(a => a.Contributions)
                                           .ThenInclude(c => c.Post)
                                  .SingleOrDefault(a => a.AuthorId == 3);
            var cToRemove = author.Contributions.SingleOrDefault(c => c.PostId == 6);
            author.Contributions.Remove(cToRemove); //remove via List<T>
            //_ctx.Remove(cToRemove); //remove using DbContext
            _ctx.ChangeTracker.DetectChanges(); //here for debugging
            _ctx.SaveChanges();
        }

        private static void RemoveJoinBetweenAuthorAndPostSimple()
        {
            var join = new Contribution { PostId = 3, AuthorId = 4 };
            _ctx.Remove(join);
            _ctx.SaveChanges();
        }

        private static void GetAuthorsForPostInMemory()
        {
            var post = _ctx.Posts.Find(4);
            _ctx.Entry(post).Collection(p => p.Contributions).Query().Include(p => p.Author).Load();

        }

        private static void GetAuthorWithPosts()
        {
            var authorWithPosts = _ctx.Authors
                .Include(a => a.Contributions)
                .ThenInclude(c => c.Post).FirstOrDefault(a => a.AuthorId == 1);
            var post = authorWithPosts.Contributions.First().Post;
            var allThePosts = new List<Post>();
            allThePosts.AddRange(authorWithPosts.Contributions.Select(c => c.Post));
        }

        private static void AddNewAuthorViaDisconnectedPostObject()
        {
            Post post;
            using (var separateOperation = new ContentPlatformContext())
            {
                post = separateOperation.Posts.Find(3);
            }
            var newAuthor = new Author { Email = "etc@etc.etc", FirstName = "Bran", LastName = "Stark", PublisherId = 1 };
            post.Contributions.Add(new Contribution { Author = newAuthor });
            _ctx.Posts.Attach(post);
            _ctx.ChangeTracker.DetectChanges();
            _ctx.SaveChanges();
        }

        private static void AddPostContributionUntracked()
        {
            Post post;
            using (var ctx2 = new ContentPlatformContext())
            {
                post = ctx2.Posts.Find(6);
            }
            
            post.Contributions.Add(new Contribution { AuthorId = 3 });
            _ctx.Posts.Attach(post);
            _ctx.ChangeTracker.DetectChanges(); // show debug info
            _ctx.SaveChanges();
        }

        private static void AddPostContribution()
        {
            var post = _ctx.Posts.Include(p => p.Contributions).FirstOrDefault(p => p.PostId == 5);
            post.Contributions.Add(new Contribution { AuthorId = 2 });
            _ctx.SaveChanges();
        }

        private static void JoinAuthorsAndPosts()
        {
            var join1 = new Contribution { AuthorId = 1, PostId = 4 };
            var join2 = new Contribution { AuthorId = 1, PostId = 5 };
            var join3 = new Contribution { AuthorId = 3, PostId = 4 };

            _ctx.AddRange(join1, join2, join3);
            _ctx.SaveChanges();
        }

        private static void PrePopulateAfterManyToMany()
        {
            _ctx.Add(
                new Blog
                {
                    BlogType = BlogType.Cooking,
                    Description = "abc",
                    PublisherId = 1,
                    Title = "Home recipes",
                    Url = "www.google.com",
                    Posts = new List<Post>
                    {
                        new Post
                        {
                            Content = "lorem ipsum",
                            Title = "Cookies",
                            TitleBackgroundColor = Color.AliceBlue
                        },
                        new Post
                        {
                            Content = "lorem ipsum 2",
                            Title = "Steak",
                            TitleBackgroundColor = Color.Red
                        },
                        new Post
                        {
                            Content = "lorem ipsum 3",
                            Title = "Fresh healthy salad",
                            TitleBackgroundColor = Color.LimeGreen
                        }
                    }
                }
            );

            _ctx.SaveChanges();
        }

        #endregion Many to many

        private static void AddEntityWithGeneratedValue()
        {
            var blog = new Blog
            {
                BlogType = BlogType.Tech,
                Description = "etc",
                PublisherId = 1,
                Title = "Fancy tech blog",
                Url = "https://emilcraciun.net/",
                Posts = new List<Post>
                    {
                        new Post
                        {
                            Content = "abc",
                            Title = "First post!"

                        }
                    }
            };

            _ctx.Blogs.Add(blog);
            _ctx.SaveChanges();
        }
    }
}