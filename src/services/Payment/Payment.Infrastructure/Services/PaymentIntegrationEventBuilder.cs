using Core.Application.Exceptions;
using Core.Application.Services;
using Core.Domain.Base;
using Messages;
using Messages.IntegrationEvents;
using Payment.Application;
using Payment.Domain.Events;

namespace Payment.Infrastructure.Services
{
    public class PaymentIntegrationEventBuilder : IIntegrationEventBuilder
    {
        private static readonly List<KeyValuePair<Type, string>> _queues = new()
        {

        };

        public IIntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is PaymentSucceeded paymentSucceeded)
                return new PaymentSucceededIE(paymentSucceeded.CorrelationId, paymentSucceeded.UserId);

            if (domainEvent is PaymentFailed paymentFailed)
                return new StockFailedIE(paymentFailed.CorrelationId, paymentFailed.UserId);

            throw new BusinessException(BusinessExceptionCodes.IntegrationEventError.GetHashCode().ToString(), BusinessExceptionCodes.IntegrationEventError.ToString());
        }

        public string GetQueueName(IIntegrationEvent integrationEvent)
        {
            return _queues.FirstOrDefault(x => x.Key == integrationEvent.GetType()).Value;
        }
    }
}
