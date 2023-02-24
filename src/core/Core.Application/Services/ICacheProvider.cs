namespace Core.Application.Services
{
    public interface ICacheProvider
    {
        public Task<T> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T item, Action<CacheSettings> settings);
        public Task RemoveAsync(string key);
    }

    public class CacheSettings
    {
        public int AbsoluteExpiration { get; set; }
        public int SlidingExpiration { get; set; }
    }
}
