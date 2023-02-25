using Core.Domain.Interfaces;

namespace Core.Infrastructure.Persistence
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly BaseDbContext _context;
        private readonly IOutboxMessageRepository _outboxMessageRepository;
        public BaseUnitOfWork(BaseDbContext context, IServiceProvider provider)
        {
            _context = context;
            _outboxMessageRepository = (IOutboxMessageRepository)provider.GetService(typeof(IOutboxMessageRepository));
        }

        public IOutboxMessageRepository OutboxMessage => _outboxMessageRepository;

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            _context.CheckDomainEvents();

            await _context.SaveChangesAsync();
        }
    }
}
