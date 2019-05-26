using System;
using System.Collections.Generic;
using System.Drawing;

namespace ContentPlatform.Domain
{
    public class Post
    {
        public Post()
        {
            Contributions = new List<Contribution>();
            Metadata = PostMetadata.Empty();
        }

        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual List<Contribution> Contributions { get; set; }
        public virtual PostMetadata Metadata { get; set; }
        public virtual Blog Blog { get; set; }
        public int BlogId { get; set; }
        public Guid Version { get; set; }
        public Color TitleBackgroundColor { get; set; }
    }
}