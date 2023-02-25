using Core.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Order.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddCoreApplication(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder builder)
        {
            builder.UseCoreApplication();

            return builder;
        }
    }
}
