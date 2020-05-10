using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using LeChuck.DataAccess.DynamoDb.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.DataAccess.Entities
{
    public class LinkDataDbEntity : IExpirableEntity, IAuditableEntity
    {
        public Guid Id { get; set; }
        public string PictureUrl { get; set; }
        public string Title { get; set; }
        public string FinalPrice { get; set; }
        public string OriginalPrice { get; set; }
        public string SavedPrice { get; set; }
        public string LongUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public List<Channel> Channels { get; set; }
        [DynamoDBProperty(StoreAsEpoch = true)]
        public DateTime? TimeToLive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
