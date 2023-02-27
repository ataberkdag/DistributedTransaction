using Core.Application.Services;
using Core.Domain.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Core.Infrastructure.Persistence
{
    public static class PersistenceExtensions
    {
        public static void CheckDomainEvents(this DbContext context, IIntegrationEventBuilder eventBuilder)
        {
            var entities = context.ChangeTracker.Entries<BaseRootEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = entities.SelectMany(x => x.Entity.DomainEvents).ToList();

            entities.ToList().ForEach(x => x.Entity.ClearDomainEvents());

            if (domainEvents == null || domainEvents.Count == 0)
                return;

            var tasks = domainEvents.Select(async (domainEvent) => {
                var integrationEvent = eventBuilder.GetIntegrationEvent(domainEvent);
                var queueName = eventBuilder.GetQueueName(integrationEvent);

                await context.Set<OutboxMessage>().AddAsync(OutboxMessage.Create(integrationEvent.GetType().AssemblyQualifiedName, JsonConvert.SerializeObject(integrationEvent), queueName));
            });

            Task.WhenAll(tasks);
        }
    }
}
