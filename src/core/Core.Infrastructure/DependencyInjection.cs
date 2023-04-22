using Consul;
using Core.Application.Services;
using Core.Infrastructure.Dependencies;
using Core.Infrastructure.DependencyModels;
using Core.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;

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

            if (dependencyOptions.Value.EnableDistributedLock)
                services.AddDistributedLock(dependencyOptions.Value.DistributedLockOptions);

            if (dependencyOptions.Value.EnableApiVersioning)
                services.AddCustomApiVersioning(dependencyOptions.Value.ApiVersioningOptions);

            if (dependencyOptions.Value.EnableHttpClient)
                services.AddCustomHttpClient();

            if (dependencyOptions.Value.EnableMessageBus)
                services.AddMessageBus(dependencyOptions.Value.MessageBusOptions);

            if (dependencyOptions.Value.EnableServiceRegistry)
            {
                services.Configure<ServiceRegistryOptions>(dependencyOptions.Value.ServiceRegistryOptions);
                services.AddSingleton<IConsulClient, ConsulClient>(provider => 
                    new ConsulClient(config => config.Address = 
                        new Uri(dependencyOptions.Value.ServiceRegistryOptions["Address"])
                    )
                );
                services.AddHostedService<ConsulRegisterService>();
            }

            if (dependencyOptions.Value.EnableAuthentication)
            {
                services.Configure<TokenOptions>(dependencyOptions.Value.TokenOptions);
                services.AddCustomAuthentication(dependencyOptions.Value.SwaggerOptions);
            }

            return services;
        }

        public static ILoggingBuilder AddCoreLogging(this ILoggingBuilder builder, IConfiguration configuration, Action<LoggingOptions> loggingOptions)
        {
            var options = new LoggingOptions();
            loggingOptions(options);

            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationIdHeader("CorrelationId");

            if (options.EnableElasticLogging)
                loggerConfiguration.WriteTo.Elasticsearch(ConfigureElasticSink(options));

            var logger = loggerConfiguration.CreateLogger();

            builder.ClearProviders();
            builder.AddSerilog(logger);

            return builder;
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(LoggingOptions loggingOptions)
        {
            return new ElasticsearchSinkOptions(new Uri(loggingOptions.ElasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{loggingOptions.ApplicationName}-{DateTime.UtcNow:yyyy}"
            };
        }
    }
}
