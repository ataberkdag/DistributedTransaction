using Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddCoreInfrastructure(opt => {
                opt.EnableDistributedCache = true;
                opt.DistributedCacheOptions = new Core.Infrastructure.Models.DistributedCacheOptions
                {
                    Database = Convert.ToInt32(configuration["DistributedCache:Database"]),
                    Endpoints = configuration["DistributedCache:Endpoints"],
                    Password = configuration["DistributedCache:Password"]
                };
            });

            return services;
        }
    }
}
