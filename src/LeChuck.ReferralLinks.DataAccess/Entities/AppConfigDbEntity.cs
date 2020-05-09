using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using LeChuck.DataAccess.DynamoDb.Interfaces;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.DataAccess.Entities
{
    [AutoMap(typeof(AppConfiguration), ReverseMap = true)]
    public class AppConfigDbEntity : IAuditableEntity
    {
        [DynamoDBHashKey] public string ConfigId { get; set; } = Constants.ConfigKey;
        public string CommandPrefix { get; set; } = "/";
        public string RootUserId { get; set; }
        public List<User> Users { get; set; }
        public List<Channel> Channels { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public long TimeToLive { get; set; }

    }
}
