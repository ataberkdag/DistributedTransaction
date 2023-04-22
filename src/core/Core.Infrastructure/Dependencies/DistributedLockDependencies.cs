using Core.Application.Services;
using Core.Infrastructure.DependencyModels;
using Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.Infrastructure.Dependencies
{
    public static class DistributedLockDependencies
    {
        public static IServiceCollection AddDistributedLock(this IServiceCollection services, DistributedLockOptions dlOptions)
        {
            if (dlOptions is null)
                return services;

            var configOption = new ConfigurationOptions
            {
                EndPoints = { },
                Password = dlOptions.Password,
                DefaultDatabase = dlOptions.Database
            };

            dlOptions.Endpoints.ForEach(endpoint => {
                configOption.EndPoints.Add(endpoint);
            });

            services.AddSingleton<ConfigurationOptions>(x => configOption);

            services.AddSingleton(typeof(Redlock.CSharp.Redlock), new Redlock.CSharp.Redlock(ConnectionMultiplexer.Connect(configOption.ToString())));

            services.AddScoped<IDistributedLockManager, DistributedLockManager>();

            return services;
        }
    }
}
