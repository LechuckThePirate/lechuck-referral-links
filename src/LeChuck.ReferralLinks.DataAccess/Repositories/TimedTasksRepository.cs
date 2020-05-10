using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LeChuck.DataAccess.DynamoDb;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.Domain;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.DataAccess.Repositories
{
    public interface ITimedTasksRepository : IDynamoDbRepository<TimedTaskDbEntity>
    {
        Task<IEnumerable<TimedTaskDbEntity>> GetPendingTasks(DateTime sweepTime);
    }

    public class TimedTasksRepository : DynamoDbRepository<TimedTaskDbEntity>, ITimedTasksRepository
    {
        public TimedTasksRepository(IAmazonDynamoDB amazonDynamoDb, ILogger<TimedTasksRepository> logger) 
            : base(Environment.GetEnvironmentVariable(Constants.EnvVarNames.TimedTasksTableName), amazonDynamoDb, logger)
        {
        }

        public async Task<IEnumerable<TimedTaskDbEntity>> GetPendingTasks(DateTime sweepTime)
        {
            var condition = new ScanCondition(nameof(TimedTaskDbEntity.NextRun), ScanOperator.LessThanOrEqual,
                DateTime.UtcNow);
            return await ScanAsync(new List<ScanCondition> {condition});
        }
    }
}
