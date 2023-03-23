using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.DependencyModels
{
    public class DependencyOptions
    {
        // DbContext Handler
        public bool EnableDbContextHandler { get; set; }

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

        public bool EnableServiceRegistry { get; set; }
        public IConfiguration? ServiceRegistryOptions { get; set; }

        // Authentication
        public bool EnableAuthentication { get; set; }
        public IConfiguration? TokenOptions { get; set; }
        public SwaggerOptions? SwaggerOptions { get; set; }
    }
}
