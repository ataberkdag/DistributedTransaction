using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Core.Infrastructure.Dependencies
{
    public static class MongoDependencies
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(s =>
                new MongoClient(configuration.GetValue<string>("MongoSettings:ConnectionString")));

            return services;
        }
    }
}
