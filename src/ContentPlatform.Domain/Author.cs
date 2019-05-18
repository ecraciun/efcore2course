using System.Collections.Generic;

namespace ContentPlatform.Domain
{
    public class Author
    {
        public Author()
        {
            Contributions = new List<Contribution>();
        }

        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName} {FirstName}";
        public string Email { get; set; }
        public List<Contribution> Contributions { get; set; }

        public Publisher Publisher { get; set; }
        public int PublisherId { get; set; }
    }
}