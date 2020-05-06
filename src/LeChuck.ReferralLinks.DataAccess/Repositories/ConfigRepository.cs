using System;
using Amazon.DynamoDBv2;
using LeChuck.DataAccess.DynamoDb;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.Domain;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.DataAccess.Repositories
{
    public interface IConfigRepository : IDynamoDbRepository<AppConfigDbEntity>
    {}

    public class ConfigRepository : DynamoDbRepository<AppConfigDbEntity>, IConfigRepository
    {
        public ConfigRepository(IAmazonDynamoDB amazonDynamoDb, ILogger<ConfigRepository> logger) 
            : base(Environment.GetEnvironmentVariable(Constants.EnvVarNames.ConfigTable), amazonDynamoDb, logger)
        { }
    }

}
