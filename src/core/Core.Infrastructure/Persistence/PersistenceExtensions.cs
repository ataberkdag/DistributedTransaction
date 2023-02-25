using Core.Domain.Base;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Core.Infrastructure.Persistence
{
    public static class PersistenceExtensions
    {
        public static void CheckDomainEvents(this DbContext context)
        {
            var entities = context.ChangeTracker.Entries<BaseRootEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = entities.SelectMany(x => x.Entity.DomainEvents).ToList();

            if (domainEvents == null || domainEvents.Count == 0)
                return;

            var tasks = domainEvents.Select(async (domainEvent) => {
                await context.Set<OutboxMessage>().AddAsync(OutboxMessage.Create(domainEvent.GetType().AssemblyQualifiedName, JsonSerializer.Serialize(domainEvent), ""));                
            });

            Task.WhenAll(tasks);

            return;
        }
    }
}
