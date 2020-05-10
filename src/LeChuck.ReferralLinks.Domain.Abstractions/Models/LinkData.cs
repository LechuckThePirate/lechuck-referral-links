using System;
using System.Collections.Generic;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class LinkData
    {
        public Guid Id { get; set; }
        public string PictureUrl { get; set; }
        public string Title { get; set; }
        public string FinalPrice { get; set; }
        public string OriginalPrice { get; set; }
        public string SavedPrice { get; set; }
        public string LongUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public List<Channel> Channels { get; set; } = new List<Channel>();

        public LinkData()
        {
            Id = Guid.NewGuid();
        }
    }
}
