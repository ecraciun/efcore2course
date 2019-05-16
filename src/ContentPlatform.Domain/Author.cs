﻿namespace ContentPlatform.Domain
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName} {FirstName}";
        public string Email { get; set; }
        //public List<Contribution> Contributions { get; set; }
    }
}