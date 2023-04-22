namespace Core.Application.Services
{
    public interface IDistributedLockManager
    {
        public Task<bool> Lock(string key, int ttlByMinute = 30);

        public Task<bool> UnLock(string key);
    }
}
