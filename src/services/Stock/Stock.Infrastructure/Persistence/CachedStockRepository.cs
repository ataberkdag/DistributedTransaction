using Core.Application.Services;
using Stock.Domain.Interfaces;
using System.Linq.Expressions;

namespace Stock.Infrastructure.Persistence
{
    public class CachedStockRepository : ICachedStockRepository
    {
        private readonly IStockRepository _decorated;
        private readonly ICacheProvider _cacheProvider;

        public const string CacheKey = "Stock_";

        public CachedStockRepository(IStockRepository decorated,
            ICacheProvider cacheProvider)
        {
            _decorated = decorated;
            _cacheProvider = cacheProvider;
        }

        public async Task Add(Domain.Entities.Stock item)
        {
            await _decorated.Add(item);
        }

        public void Delete(Domain.Entities.Stock item)
        {
            _decorated.Delete(item);
        }

        public async Task<List<Domain.Entities.Stock>> FindByQuery(Expression<Func<Domain.Entities.Stock, bool>> expression, bool tracking = true)
        {
            return await _decorated.FindByQuery(expression, tracking);
        }

        public async Task<Domain.Entities.Stock> GetById(long id)
        {
            var result = await _cacheProvider.GetAsync<Domain.Entities.Stock>($"{CacheKey}{id}");
            if (result != null)
                return result;

            result = await _decorated.GetById(id);
            if (result is null)
                return default(Domain.Entities.Stock);

            await _cacheProvider.SetAsync<Domain.Entities.Stock>($"{CacheKey}{id}", result, x => {
                x.AbsoluteExpiration = 30;
                x.SlidingExpiration = 30;
            });

            return result;
        }
    }
}
