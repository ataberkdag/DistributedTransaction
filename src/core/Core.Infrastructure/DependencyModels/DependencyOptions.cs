namespace Core.Infrastructure.DependencyModels
{
    public class DependencyOptions
    {
        // Distributed Cache
        public bool EnableDistributedCache { get; set; }
        public DistributedCacheOptions? DistributedCacheOptions { get; set; }

        // Api Versioning
        public bool EnableApiVersioning { get; set; }
        public CustomApiVersioningOptions? ApiVersioningOptions { get; set; }

        // Http Client Factory
        public bool EnableHttpClient { get; set; }

        // Message Bus - Broker
        public bool EnableMessageBus { get; set; }
        public MessageBusOptions? MessageBusOptions { get; set; }
    }
}
