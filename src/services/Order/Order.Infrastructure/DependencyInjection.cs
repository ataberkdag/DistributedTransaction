using Core.Infrastructure;
using Core.Infrastructure.DependencyModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Models;
using Order.Application.Services;
using Order.Domain.Interfaces;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Services;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddDbContext<OrderDbContext>(opt => {
                opt.UseNpgsql(configuration.GetConnectionString("Database"));
            });

            services.AddCoreInfrastructure(opt => {

                // Distributed Cache
                opt.EnableDistributedCache = true;
                opt.DistributedCacheOptions = new Core.Infrastructure.DependencyModels.DistributedCacheOptions
                {
                    Database = Convert.ToInt32(configuration["DistributedCache:Database"]),
                    Endpoints = configuration["DistributedCache:Endpoints"],
                    Password = configuration["DistributedCache:Password"]
                };

                // Api Versioning
                opt.EnableApiVersioning = true;
                opt.ApiVersioningOptions = new Core.Infrastructure.DependencyModels.CustomApiVersioningOptions
                {
                    ApiVersionReaders = new List<CustomApiVersionReader> { CustomApiVersionReader.Url},
                    DefaultApiVersion = 1,
                    EnableReportApiVersion = true
                };

                // Http Client
                opt.EnableHttpClient = true;
            });

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();

            services.AddSingleton<UserServiceConfig>(new UserServiceConfig {
                BaseUrl = configuration["ExternalUrls:UserService"]
            });

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
