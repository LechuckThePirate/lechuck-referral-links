﻿using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using LeChuck.DataAccess.DynamoDb.Interfaces;

namespace LeChuck.ReferralLinks.DataAccess.Entities
{
    public class StateMachineDbEntity : IExpirableEntity
    {
        [DynamoDBHashKey]
        public string MachineId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        [DynamoDBProperty(StoreAsEpoch = true)]
        public DateTime? TimeToLive { get; set; }
    }
}
