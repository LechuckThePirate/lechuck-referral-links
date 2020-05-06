using System;
using Amazon.DynamoDBv2.DataModel;
using LeChuck.DataAccess.DynamoDb.Interfaces;

namespace LeChuck.ReferralLinks.DataAccess.Entities
{
    public class AppConfigDbEntity : IAuditableEntity
    {
        [DynamoDBHashKey]
        public string ConfigId { get; set; } = nameof(AppConfigDbEntity);
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public long TimeToLive { get; set; }
    }
}
