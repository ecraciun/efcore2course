using System;
using System.Collections.Generic;

namespace ContentPlatform.Domain
{
    public class Publisher
    {
        private List<Blog> _blogs;

        public Publisher()
        {
            Blogs = new List<Blog>();
            Authors = new List<Author>();
        }

        private Publisher(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private Action<object, string> LazyLoader { get; set; }

        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string MainWebsite { get; set; }
        public List<Blog> Blogs
        {
            get => LazyLoader.Load(this, ref _blogs);
            set => _blogs = value;
        }
        public virtual List<Author> Authors { get; set; }
        public virtual Location MainOffice { get; set; }
    }
}