namespace Core.Infrastructure.Models
{
    public class DependencyOptions
    {
        public string ConnectionString { get; set; }
        public bool EnableDistributedCache { get; set; }
        public DistributedCacheOptions? DistributedCacheOptions { get; set; }
    }
}
