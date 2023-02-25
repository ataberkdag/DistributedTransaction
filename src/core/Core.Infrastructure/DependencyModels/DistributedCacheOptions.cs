using Core.Application.Common;

namespace Core.Infrastructure.DependencyModels
{
    public class DistributedCacheOptions
    {
        public string Endpoints { get; set; }
        public string Password { get; set; }
        public int Database { get; set; }
    }
}
