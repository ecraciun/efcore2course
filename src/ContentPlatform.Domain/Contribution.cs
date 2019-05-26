namespace ContentPlatform.Domain
{
    public class Contribution
    {
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}