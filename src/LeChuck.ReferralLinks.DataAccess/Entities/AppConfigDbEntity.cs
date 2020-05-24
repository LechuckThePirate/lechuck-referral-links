#region using directives

using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using LeChuck.DataAccess.DynamoDb.Interfaces;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

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
        public List<VendorConfig> VendorServices { get; set; } = new List<VendorConfig>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public long TimeToLive { get; set; }
        public string DefaultShortener { get; set; } = Constants.Providers.Shorteners.None;

    }
}