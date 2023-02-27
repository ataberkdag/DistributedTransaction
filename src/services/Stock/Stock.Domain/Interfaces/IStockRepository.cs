using Core.Domain.Interfaces;

namespace Stock.Domain.Interfaces
{
    public interface IStockRepository : IGenericRepository<Stock.Domain.Entities.Stock>
    {
    }
}
