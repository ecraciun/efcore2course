using System.Collections.Generic;

namespace ContentPlatform.Domain
{
    public class Publisher
    {
        public Publisher()
        {
            Blogs = new List<Blog>();
            Authors = new List<Author>();
        }

        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string MainWebsite { get; set; }
        public virtual List<Blog> Blogs { get; set; }
        public virtual List<Author> Authors { get; set; }
        public virtual Location MainOffice { get; set; }
    }
}