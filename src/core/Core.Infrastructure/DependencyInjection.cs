using Core.Infrastructure.Dependencies;
using Core.Infrastructure.DependencyModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, Action<DependencyOptions> options)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddOptions<DependencyOptions>().Configure(options);

            var dependencyOptions = services.BuildServiceProvider().GetService<IOptions<DependencyOptions>>();

            if (dependencyOptions.Value.EnableDistributedCache)
                services.AddDistributedCache(dependencyOptions.Value.DistributedCacheOptions);

            if (dependencyOptions.Value.EnableApiVersioning)
                services.AddCustomApiVersioning(dependencyOptions.Value.ApiVersioningOptions);

            if (dependencyOptions.Value.EnableHttpClient)
                services.AddCustomHttpClient();

            return services;
        }

    }
}
