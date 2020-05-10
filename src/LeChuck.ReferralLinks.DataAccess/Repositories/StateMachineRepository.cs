using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2;
using LeChuck.DataAccess.DynamoDb;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.Domain;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.DataAccess.Repositories
{
    public interface IStateMachineRepository : IDynamoDbRepository<StateMachineDbEntity> { }

    public class StateMachineRepository : DynamoDbRepository<StateMachineDbEntity>, IStateMachineRepository
    {
        public StateMachineRepository(IAmazonDynamoDB amazonDynamoDb, ILogger<StateMachineRepository> logger) 
            : base(Environment.GetEnvironmentVariable(Constants.EnvVarNames.StateMachineTableName), amazonDynamoDb, logger)
        {
        }
    }
}
