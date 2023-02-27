using Core.Infrastructure;
using Core.Infrastructure.DependencyModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Services;

namespace Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddDbContext<StockDbContext>(opt => {
                opt.UseNpgsql(configuration.GetConnectionString("Database"));
            });

            services.AddCoreInfrastructure(opt => {

                // Distributed Cache
                opt.EnableDistributedCache = true;
                opt.DistributedCacheOptions = new Core.Infrastructure.DependencyModels.DistributedCacheOptions
                {
                    Database = Convert.ToInt32(configuration["DistributedCache:Database"]),
                    Endpoints = configuration["DistributedCache:Endpoints"],
                    Password = configuration["DistributedCache:Password"]
                };

                // Message Broker - Bus
                opt.EnableMessageBus = true;
                opt.MessageBusOptions = new MessageBusOptions
                {
                    Host = configuration["MessageBroker:Host"],
                    UserName = configuration["MessageBroker:UserName"],
                    Password = configuration["MessageBroker:Password"],
                    Consumers = (cfg) =>
                    {
                        // TODO: Consumers will add.
                        //cfg.AddConsumer<>();

                        return cfg;
                    },
                    Endpoints = (cfg) =>
                    {
                        // TODO: Receive Endpoints will add.
                        //cfg.ReceiveEndpoint();
                    },
                    IntegrationEventBuilderType = typeof(StockIntegrationEventBuilder)
                };
            });

            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockUnitOfWork, StockUnitOfWork>();

            return services;
        }

        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.AddCoreLogging(configuration);

            return builder;
        }
    }
}
