using Core.Infrastructure;
using Core.Infrastructure.DependencyModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.Domain.Interfaces;
using Payment.Infrastructure.Persistence;

namespace Payment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, Action<MessageBusOptions> messageBusOptions = null)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddDbContext<PaymentDbContext>(opt => {
                opt.UseNpgsql(configuration.GetConnectionString("Database"));
            });

            services.AddCoreInfrastructure(opt => {

                // Distributed Cache
                /*
                opt.EnableDistributedCache = true;
                opt.DistributedCacheOptions = new Core.Infrastructure.DependencyModels.DistributedCacheOptions
                {
                    Database = Convert.ToInt32(configuration["DistributedCache:Database"]),
                    Endpoints = configuration["DistributedCache:Endpoints"],
                    Password = configuration["DistributedCache:Password"]
                };*/

                // Message Broker - Bus
                if (messageBusOptions != null)
                {
                    opt.EnableMessageBus = true;

                    services.AddOptions<MessageBusOptions>().Configure(messageBusOptions);

                    var messageBusOpt = services.BuildServiceProvider().GetService<IOptions<MessageBusOptions>>();

                    opt.MessageBusOptions = messageBusOpt.Value;
                }
            });

            services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
            services.AddScoped<IPaymentUnitOfWork, PaymentUnitOfWork>();

            return services;
        }

        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.AddCoreLogging(configuration);

            return builder;
        }
    }
}
