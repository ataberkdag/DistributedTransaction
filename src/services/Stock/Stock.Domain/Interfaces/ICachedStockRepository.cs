using Core.Domain.Interfaces;

namespace Stock.Domain.Interfaces
{
    public interface ICachedStockRepository : IGenericRepository<Stock.Domain.Entities.Stock>
    {
    }
}
