using Core.Application.Exceptions;
using Core.Application.Services;
using Core.Domain.Base;
using Messages;
using Messages.IntegrationEvents;
using Order.Domain.Events;

namespace Order.Infrastructure.Services
{
    public class OrderIntegrationEventBuilder : IIntegrationEventBuilder
    {
        private static readonly List<KeyValuePair<Type, string>> _queues = new()
        {

        };

        public IIntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is OrderPlaced orderPlaced)
            {
                return new OrderPlacedIE(orderPlaced.CorrelationId, orderPlaced.UserId, orderPlaced.OrderItems.Select(x => new OrderItemDto(x.ItemId, x.Quantity)).ToList());
            }

            throw new BusinessException("8888", "Integration Event Error");
        }

        public string GetQueueName(IIntegrationEvent integrationEvent)
        {
            return _queues.FirstOrDefault(x => x.Key == integrationEvent.GetType()).Value;
        }
    }
}
