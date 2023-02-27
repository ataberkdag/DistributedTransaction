using Core.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Stock.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddCoreApplication(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
