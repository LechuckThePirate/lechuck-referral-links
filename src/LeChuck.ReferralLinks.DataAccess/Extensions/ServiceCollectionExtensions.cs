using Amazon.DynamoDBv2;
using AutoMapper;
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
            services.AddAutoMapper(configAction: cfg => { }
                , typeof(ServiceCollectionExtensions).Assembly);
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddInherited<IDynamoDbRepository>(typeof(ServiceCollectionExtensions).Assembly);
            services.AddInherited<IUnitOfWork>(typeof(ServiceCollectionExtensions).Assembly);
            return services;
        }
    }
}
