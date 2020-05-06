using System.Runtime.CompilerServices;
using Amazon.DynamoDBv2;
using LeChuck.DataAccess.DynamoDb;
using LeChuck.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeChuck.ReferralLinks.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddInherited<IDynamoDbRepository>(typeof(ServiceCollectionExtensions).Assembly);
            services.AddInherited<IUnitOfWork>(typeof(ServiceCollectionExtensions).Assembly);
            return services;
        }
    }
}
