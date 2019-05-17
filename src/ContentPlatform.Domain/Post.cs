using System;
using System.Collections.Generic;

namespace ContentPlatform.Domain
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public List<Contribution> Contributions { get; set; }
        //public PostMetadata Metadata { get; set; }
        public Blog Blog { get; set; }
        public int BlogId { get; set; }
        public Guid Version { get; set; }
    }
}