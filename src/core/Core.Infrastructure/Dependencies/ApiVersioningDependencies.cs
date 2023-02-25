using Core.Infrastructure.DependencyModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Dependencies
{
    public static class ApiVersioningDependencies
    {
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services, CustomApiVersioningOptions options)
        {
            services.AddApiVersioning(opt => {
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(options.DefaultApiVersion, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;

                if (options.EnableReportApiVersion)
                    opt.ReportApiVersions = true;

                foreach (var reader in options.ApiVersionReaders)
                {
                    if (reader == CustomApiVersionReader.Url)
                        opt.ApiVersionReader = new UrlSegmentApiVersionReader();
                }
            });

            return services;
        }
    }
}
