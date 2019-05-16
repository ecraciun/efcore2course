namespace ContentPlatform.Domain
{
    public class Contribution
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}