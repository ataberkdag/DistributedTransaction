using MassTransit;

namespace Core.Infrastructure.DependencyModels
{
    public class MessageBusOptions
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public Func<IBusRegistrationConfigurator, IRegistrationConfigurator> Consumers { get; set; }
        public Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> Endpoints { get; set; }

        public bool IsProducer { get; set; }

        public Type IntegrationEventBuilderType { get; set; }
    }
}
