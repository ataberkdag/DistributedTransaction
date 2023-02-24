namespace Core.Domain.Interfaces
{
    public interface IBaseUnitOfWork : IDisposable
    {
        IOutboxMessageRepository OutboxMessage { get; }
        Task SaveChangesAsync();
    }
}
