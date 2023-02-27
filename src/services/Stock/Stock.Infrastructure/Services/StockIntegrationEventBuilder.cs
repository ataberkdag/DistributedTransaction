using Core.Application.Exceptions;
using Core.Application.Services;
using Core.Domain.Base;
using Messages;
using Messages.IntegrationEvents;
using Stock.Domain.Events;

namespace Stock.Infrastructure.Services
{
    public class StockIntegrationEventBuilder : IIntegrationEventBuilder
    {
        private static readonly List<KeyValuePair<Type, string>> _queues = new()
        {

        };

        public IIntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is StockDecreased stockDecreased)
                return new StockDecreasedIE(stockDecreased.CorrelationId, stockDecreased.UserId, stockDecreased.TotalAmount);

            if (domainEvent is StockFailed stockFailed)
                return new StockFailedIE(stockFailed.CorrelationId, stockFailed.UserId);

            throw new BusinessException("8888", "Integration Event Error");
        }

        public string GetQueueName(IIntegrationEvent integrationEvent)
        {
            return _queues.FirstOrDefault(x => x.Key == integrationEvent.GetType()).Value;
        }
    }
}
