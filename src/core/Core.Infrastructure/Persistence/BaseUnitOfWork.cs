using Core.Application.Services;
using Core.Domain.Interfaces;

namespace Core.Infrastructure.Persistence
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly BaseDbContext _context;
        private readonly IOutboxMessageRepository _outboxMessageRepository;
        private readonly IIntegrationEventBuilder _integrationEventBuilder;
        public BaseUnitOfWork(BaseDbContext context, IServiceProvider provider, IIntegrationEventBuilder integrationEventBuilder)
        {
            _context = context;
            _outboxMessageRepository = (IOutboxMessageRepository)provider.GetService(typeof(IOutboxMessageRepository));
            _integrationEventBuilder = integrationEventBuilder;
        }

        public IOutboxMessageRepository OutboxMessage => _outboxMessageRepository;

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            _context.CheckDomainEvents(_integrationEventBuilder);

            await _context.SaveChangesAsync();
        }
    }
}
