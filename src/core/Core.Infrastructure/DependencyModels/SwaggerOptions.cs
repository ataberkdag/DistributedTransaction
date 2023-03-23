using System.Reflection;

namespace Core.Infrastructure.DependencyModels
{
    public class SwaggerOptions
    {
        public bool EnableSwaggerGen { get; set; } = true;
        public string BaseDirectory { get; set; }
        public Assembly ApiAssembly { get; set; }
    }
}
