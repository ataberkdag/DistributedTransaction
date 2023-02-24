using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            return services;
        }

    }
}
