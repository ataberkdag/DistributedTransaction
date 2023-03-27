using Core.Infrastructure;
using Core.Infrastructure.DependencyModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Report.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, Action<MessageBusOptions> messageBusOptions = null)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddCoreInfrastructure(opt => {

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
            });

            return services;
        }

        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.AddCoreLogging(configuration, options => {
                options.EnableElasticLogging = true;
                options.ApplicationName = "Report";
                options.ElasticUri = configuration["ElasticConfiguration:Uri"];
            });

            return builder;
        }
    }
}
