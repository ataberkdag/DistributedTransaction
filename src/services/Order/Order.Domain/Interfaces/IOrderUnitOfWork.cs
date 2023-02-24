using Core.Domain.Interfaces;

namespace Order.Domain.Interfaces
{
    public interface IOrderUnitOfWork : IBaseUnitOfWork
    {
        IOrderRepository Orders { get; }
    }
}
