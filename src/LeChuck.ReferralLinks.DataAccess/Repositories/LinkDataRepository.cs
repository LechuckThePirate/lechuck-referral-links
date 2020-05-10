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
    public interface ILinkDataRepository : IDynamoDbRepository<LinkDataDbEntity>
    {

    }

    public class LinkDataRepository : DynamoDbRepository<LinkDataDbEntity>, ILinkDataRepository
    {
        public LinkDataRepository(IAmazonDynamoDB amazonDynamoDb, ILogger<LinkDataRepository> logger) 
            : base(Environment.GetEnvironmentVariable(Constants.EnvVarNames.LinkDataTableName), amazonDynamoDb, logger)
        {
        }
    }
}
