using Core.Domain.Base;
using Messages;

namespace Core.Application.Services
{
    public interface IIntegrationEventBuilder
    {
        public IIntegrationEvent GetIntegrationEvent(IDomainEvent domainEvent);
        public string GetQueueName(IIntegrationEvent integrationEvent);
    }
}
