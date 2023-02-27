using Core.Domain.Interfaces;

namespace Stock.Domain.Interfaces
{
    public interface IStockUnitOfWork : IBaseUnitOfWork
    {
        IStockRepository Stocks { get; }
    }
}
