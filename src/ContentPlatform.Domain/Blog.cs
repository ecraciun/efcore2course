using System;
using System.Collections.Generic;

namespace ContentPlatform.Domain
{
    public class Blog
    {
        public Blog()
        {
            Posts = new List<Post>();
        }

        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public BlogType BlogType { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual Publisher Publisher { get; set; }
        public int PublisherId { get; set; }
        public DateTime TakeDownTime { get; set; }
        public BlogMetadata BlogMetadata { get; set; }
    }

    public class BlogMetadata
    {
        public string Something { get; set; }
    }
}