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
                //AddEntityWithGeneratedValue();
                //RunManyToManyExamples();
                //RunOneToOneExamples();
            }
        }


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