namespace Core.Infrastructure.Models
{
    public class CustomApiVersioningOptions
    {
        public int DefaultApiVersion { get; set; }
        public bool EnableReportApiVersion { get; set; }
        public List<CustomApiVersionReader> ApiVersionReaders { get; set; }
        
    }

    public enum CustomApiVersionReader
    {
        Url = 1
    }
}
