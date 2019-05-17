using System.Collections.Generic;

namespace ContentPlatform.Domain
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public BlogType BlogType { get; set; }
        public List<Post> Posts { get; set; }
        public Publisher Publisher { get; set; }
        public int PublisherId { get; set; }
    }
}