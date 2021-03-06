﻿namespace ContentPlatform.Domain
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Address { get; set; }
        public virtual Publisher Publisher { get; set; }
        public int? PublisherId { get; set; }
    }
}