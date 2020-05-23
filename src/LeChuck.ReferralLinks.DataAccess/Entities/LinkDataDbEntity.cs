#region using directives

using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using LeChuck.DataAccess.DynamoDb.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.DataAccess.Entities
{
    public class MultiLinkDbEntity : IExpirableEntity, IAuditableEntity
    {
        public Guid Id { get; set; }
        public List<LinkMessage> Links { get; set; }
        public List<Channel> Channels { get; set; }
        public int LastMessageSent { get; set; }

        [DynamoDBProperty(StoreAsEpoch = true)]
        public DateTime? TimeToLive { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime NextRun { get; set; }
        public TimeSpan RunSpan { get; set; }
    }
}