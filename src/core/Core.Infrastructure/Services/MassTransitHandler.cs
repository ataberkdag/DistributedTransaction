using Core.Infrastructure.Services.Interfaces;
using MassTransit;

namespace Core.Infrastructure.Services
{
    public class MassTransitHandler : IMassTransitHandler
    {
        private readonly IServiceProvider _serviceProvider;
        public MassTransitHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Publish(object @event)
        {
            var bus = (IBus)this._serviceProvider.GetService(typeof(IBus));
            await bus.Publish(@event);
        }

        public async Task Send(string queueName, object @event)
        {
            var bus = (IBus)this._serviceProvider.GetService(typeof(IBus));
            var uri = new Uri($"{bus.Address.Scheme}://{bus.Address.Authority}/{queueName}");
            var endPoint = await bus.GetSendEndpoint(uri);

            await endPoint.Send(@event);
        }
    }
}
