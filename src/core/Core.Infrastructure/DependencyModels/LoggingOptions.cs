namespace Core.Infrastructure.DependencyModels
{
    public class LoggingOptions
    {
        public bool EnableElasticLogging { get; set; }
        public string ElasticUri { get; set; }
        public string ApplicationName { get; set; }
    }
}
