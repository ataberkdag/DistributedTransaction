using Core.Application.Services;
using Core.Infrastructure.Dependencies;
using Core.Infrastructure.DependencyModels;
using Core.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace Core.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, Action<DependencyOptions> options)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddHttpContextAccessor();

            services.AddOptions<DependencyOptions>().Configure(options);

            var dependencyOptions = services.BuildServiceProvider().GetService<IOptions<DependencyOptions>>();

            // TODO: Bug
            if (dependencyOptions.Value.EnableDbContextHandler)
                services.AddScoped<IDbContextHandler, DbContextHandler>();

            if (dependencyOptions.Value.EnableDistributedCache)
                services.AddDistributedCache(dependencyOptions.Value.DistributedCacheOptions);

            if (dependencyOptions.Value.EnableApiVersioning)
                services.AddCustomApiVersioning(dependencyOptions.Value.ApiVersioningOptions);

            if (dependencyOptions.Value.EnableHttpClient)
                services.AddCustomHttpClient();

            if (dependencyOptions.Value.EnableMessageBus)
                services.AddMessageBus(dependencyOptions.Value.MessageBusOptions);

            return services;
        }

        public static ILoggingBuilder AddCoreLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationIdHeader("CorrelationId")
                .CreateLogger();

            builder.ClearProviders();
            builder.AddSerilog(logger);

            return builder;
        }
    }
}
