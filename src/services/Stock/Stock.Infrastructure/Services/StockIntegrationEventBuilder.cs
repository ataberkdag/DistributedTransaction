using Core.Application.Services;
using Core.Domain.Base;
using Messages;
using Messages.IntegrationEvents;

namespace Stock.Infrastructure.Services
{
    public class StockIntegrationEventBuilder : IIntegrationEventBuilder
    {
        private static readonly List<KeyValuePair<Type, string>> _queues = new()
        {
            new KeyValuePair<Type, string>(typeof(OrderPlacedIE), "Order_Placed")
        };

        public IIntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        public string GetQueueName(IIntegrationEvent integrationEvent)
        {
            return _queues.FirstOrDefault(x => x.Key == integrationEvent.GetType()).Value;
        }
    }
}
