using Core.Application.Services;
using StackExchange.Redis;

namespace Core.Infrastructure.Services
{
    public class DistributedLockManager : IDistributedLockManager
    {
        private readonly Redlock.CSharp.Redlock _dlm;
        private readonly ConfigurationOptions _configurationOptions;

        public DistributedLockManager(Redlock.CSharp.Redlock dlm,
            ConfigurationOptions configurationOptions)
        {
            _dlm = dlm;
            _configurationOptions = configurationOptions;
        }


        public async Task<bool> Lock(string key, int ttlByMinute = 30)
        {
            var success = await _dlm.LockAsync(key, TimeSpan.FromMinutes(ttlByMinute));

            return success.success;
        }

        public async Task<bool> UnLock(string key)
        {
            RedisValue redLockValue;
            using (var redis = ConnectionMultiplexer.Connect(_configurationOptions.ToString()))
            {
                redLockValue = await redis.GetDatabase().StringGetAsync(key);
            }

            if (redLockValue.IsNull)
                return false;

            await _dlm.UnlockAsync(new Redlock.CSharp.Lock(key, redLockValue, default));

            return true;
        }
    }
}
