using System;
using System.Collections.Generic;

namespace ContentPlatform.Domain
{
    public class PostMetadata
    {
        public static PostMetadata Create(string title, string keywords)
        {
            return new PostMetadata(title, keywords);
        }

        public static PostMetadata Empty()
        {
            return new PostMetadata("", "");
        }

        private PostMetadata(string title, string keywords)
        {
            Title = title;
            Keywords = keywords;
        }
        private PostMetadata() { }

        public string Title { get; set; }
        public string Keywords { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Keywords);
        }

        public override bool Equals(object obj)
        {
            return obj is PostMetadata metadata &&
                   Title == metadata.Title &&
                   Keywords == metadata.Keywords;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Keywords);
        }

        public static bool operator ==(PostMetadata left, PostMetadata right)
        {
            return EqualityComparer<PostMetadata>.Default.Equals(left, right);
        }

        public static bool operator !=(PostMetadata left, PostMetadata right)
        {
            return !(left == right);
        }
    }
}