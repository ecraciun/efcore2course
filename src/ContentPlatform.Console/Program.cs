using ContentPlatform.Data;
using ContentPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Console = System.Console;

namespace ContentPlatform.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            Migrate();
            //AddEntityWithGeneratedValue();
        }

        private static void Migrate()
        {
            using (var ctx = new ContentPlatformContext())
            {
                ctx.Database.Migrate();
            }
        }

        private static void AddEntityWithGeneratedValue()
        {
            using (var ctx = new ContentPlatformContext())
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

                ctx.Blogs.Add(blog);
                ctx.SaveChanges();
            }
        }
    }
}