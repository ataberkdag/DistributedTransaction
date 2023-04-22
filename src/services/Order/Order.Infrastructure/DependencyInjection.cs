using Core.Infrastructure;
using Core.Infrastructure.DependencyModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Order.Application.Models;
using Order.Application.Services;
using Order.Domain.Interfaces;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Services;
using System.Reflection;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, 
            Assembly executingAssembly,
            string baseDirectory,
            Action<MessageBusOptions> messageBusOptions = null)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddDbContext<OrderDbContext>(opt => {
                opt.UseNpgsql(configuration.GetConnectionString("Database"));
            });

            services.AddCoreInfrastructure(opt => {

                // DbContext Handler
                opt.EnableDbContextHandler = false;

                // Distributed Cache
                opt.EnableDistributedCache = true;
                opt.DistributedCacheOptions = new Core.Infrastructure.DependencyModels.DistributedCacheOptions
                {
                    Database = Convert.ToInt32(configuration["DistributedCache:Database"]),
                    Endpoints = configuration["DistributedCache:Endpoints"],
                    Password = configuration["DistributedCache:Password"]
                };

                // Distributed Lock
                opt.EnableDistributedLock = true;
                services.Configure<DistributedLockOptions>(opt => configuration.GetSection("DistributedLock").Bind(opt));
                opt.DistributedLockOptions = services.BuildServiceProvider().GetRequiredService<IOptions<DistributedLockOptions>>().Value;

                // Api Versioning
                opt.EnableApiVersioning = true;
                opt.ApiVersioningOptions = new Core.Infrastructure.DependencyModels.CustomApiVersioningOptions
                {
                    ApiVersionReaders = new List<CustomApiVersionReader> { CustomApiVersionReader.Url},
                    DefaultApiVersion = 1,
                    EnableReportApiVersion = true
                };

                // Http Client
                opt.EnableHttpClient = true;

                // Message Broker - Bus
                if (messageBusOptions != null)
                {
                    opt.EnableMessageBus = true;

                    services.AddOptions<MessageBusOptions>().Configure(messageBusOptions);

                    var messageBusOpt = services.BuildServiceProvider().GetService<IOptions<MessageBusOptions>>();

                    opt.MessageBusOptions = messageBusOpt.Value;
                }

                // Service Registry - Consul
                opt.EnableServiceRegistry = true;
                opt.ServiceRegistryOptions = configuration.GetSection("ServiceRegistry");

                // Authentication
                opt.EnableAuthentication = true;
                opt.TokenOptions = configuration.GetSection("TokenOptions");
                opt.SwaggerOptions = new SwaggerOptions
                {
                    ApiAssembly = executingAssembly,
                    BaseDirectory = baseDirectory
                };
            });

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();

            services.AddSingleton<LimitServiceConfig>(new LimitServiceConfig {
                BaseUrl = configuration["ExternalUrls:LimitService"]
            });

            services.AddScoped<ILimitService, LimitService>();

            return services;
        }

        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.AddCoreLogging(configuration, options => { 
                options.EnableElasticLogging = true;
                options.ApplicationName = "Order";
                options.ElasticUri = configuration["ElasticConfiguration:Uri"];
            });

            return builder;
        }
    }
}
