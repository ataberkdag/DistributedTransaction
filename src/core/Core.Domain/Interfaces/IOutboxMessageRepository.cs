using Core.Domain.Entities;

namespace Core.Domain.Interfaces
{
    public interface IOutboxMessageRepository : IGenericRepository<OutboxMessage>
    {
    }
}
