namespace Core.Infrastructure.DependencyModels
{
    public class DistributedLockOptions
    {
        public List<string> Endpoints { get; set; }
        public string Password { get; set; }
        public int Database { get; set; }
    }
}
