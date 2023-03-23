using Core.Infrastructure;
using Core.Infrastructure.DependencyModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using User.BL.Services.Impl;
using User.BL.Services.Interfaces;

namespace User.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBL(this IServiceCollection services, 
            IConfiguration configuration, 
            Assembly executingAssembly,
            string baseDirectory)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddScoped<IDbContextHandler, DbContextHandler>();

            services.AddMediatR(x => {
                x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddCoreInfrastructure(opt => {
                opt.EnableServiceRegistry = true;
                opt.ServiceRegistryOptions = configuration.GetSection("ServiceRegistry");

                opt.EnableAuthentication = true;
                opt.TokenOptions = configuration.GetSection("TokenOptions");
                opt.SwaggerOptions = new SwaggerOptions
                {
                    ApiAssembly = executingAssembly,
                    BaseDirectory = baseDirectory
                };
            });

            services.AddScoped<IAccessTokenService, AccessTokenService>();

            return services;
        }
    }
}
