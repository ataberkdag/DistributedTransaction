using Core.Application.Services;
using Core.Domain.Interfaces;
using Core.Infrastructure.Persistence;
using Stock.Domain.Interfaces;

namespace Stock.Infrastructure.Persistence
{
    public class StockUnitOfWork : BaseUnitOfWork, IStockUnitOfWork
    {
        private readonly IStockRepository _stocks;
        public StockUnitOfWork(StockDbContext context, IServiceProvider provider, IIntegrationEventBuilder eventBuilder) : base(context, provider, eventBuilder)
        {
            _stocks = (IStockRepository)provider.GetService(typeof(IStockRepository));
        }

        public IStockRepository Stocks => _stocks;
    }
}
