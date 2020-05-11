using System;
using System.Collections.Generic;
using System.Text;
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
    public interface IMultiLinkRepository : IDynamoDbRepository<MultiLinkDbEntity>
    {
        Task<IEnumerable<MultiLinkDbEntity>> GetPendingTasks(DateTime sweepTime);
    }

    public class MultiMultiLinkRepository : DynamoDbRepository<MultiLinkDbEntity>, IMultiLinkRepository
    {
        public MultiMultiLinkRepository(IAmazonDynamoDB amazonDynamoDb, ILogger<MultiMultiLinkRepository> logger) 
            : base(Environment.GetEnvironmentVariable(Constants.EnvVarNames.LinkDataTableName), amazonDynamoDb, logger)
        {
        }

        public async Task<IEnumerable<MultiLinkDbEntity>> GetPendingTasks(DateTime sweepTime)
        {
            var condition = new ScanCondition(nameof(MultiLinkDbEntity.NextRun), ScanOperator.LessThanOrEqual,
                DateTime.UtcNow);
            return await ScanAsync(new List<ScanCondition> { condition });
        }

    }
}
