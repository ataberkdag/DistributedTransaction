using Core.Infrastructure.Persistence;
using Stock.Domain.Interfaces;

namespace Stock.Infrastructure.Persistence
{
    public class StockRepository : GenericRepository<Stock.Domain.Entities.Stock>, IStockRepository
    {
        public StockRepository(StockDbContext context) : base(context)
        {
        }
    }
}
