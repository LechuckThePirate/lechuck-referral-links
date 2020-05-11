using System;
using System.Collections.Generic;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class MultiLink
    {
        public Guid Id { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
        public List<Channel> Channels { get; set; } = new List<Channel>();
        public int LastMessageSent { get; set; }
        public MultiLink()
        {
            Id = Guid.NewGuid();
        }
    }

    public class Link
    {
        public int Number { get; set; }
        public string PictureUrl { get; set; }
        public string Title { get; set; }
        public string FinalPrice { get; set; }
        public string OriginalPrice { get; set; }
        public string SavedPrice { get; set; }
        public string LongUrl { get; set; }
        public string ShortenedUrl { get; set; }
    }
}
