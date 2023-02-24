using Core.Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Core.Infrastructure.Services
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _cache;

        public CacheProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var cacheItem = await _cache.GetAsync(key);

            if (cacheItem == null)
                return default(T);

            if (typeof(T).IsValueType)
                return (T)Convert.ChangeType(cacheItem, typeof(T));

            return JsonSerializer.Deserialize<T>(cacheItem);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T item, Action<CacheSettings> settings)
        {
            string stringItem;

            if (typeof(T).IsValueType)
                stringItem = item.ToString();
            else
                stringItem = JsonSerializer.Serialize(item);

            var cacheSettings = new CacheSettings();

            settings(cacheSettings);

            await _cache.SetStringAsync(key, stringItem, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(cacheSettings.AbsoluteExpiration),
                SlidingExpiration = TimeSpan.FromMinutes(cacheSettings.SlidingExpiration)
            });
        }
    }
}
