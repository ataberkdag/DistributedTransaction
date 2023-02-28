using Core.Application.Services;
using Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Core.Infrastructure.Dependencies
{
    public static class HttpClientDependencies
    {
        public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("client")
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)))
                .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(10)));

            services.AddScoped<IHttpCaller, HttpCaller>();

            return services;
        }
    }
}
