using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using LeChuck.DataAccess.DynamoDb.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.DataAccess.Entities
{
    public class TimedTaskDbEntity : IExpirableEntity, IAuditableEntity
    {
        [DynamoDBHashKey]
        public Guid TaskId { get; set; }

        public DateTime NextRun { get; set; }
        public string CronPattern { get; set; }
        public LinkData Message { get; set; }
        public List<Channel> Channels { get; set; }
        public long TimeToLive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
