﻿using Core.Infrastructure;
using Core.Infrastructure.DependencyModels;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Order.Application.Models;
using Order.Application.Services;
using Order.Domain.Interfaces;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Services;
using Serilog;

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

                // Message Broker - Bus
                opt.EnableMessageBus = true;
                opt.MessageBusOptions = new MessageBusOptions
                {
                    Host = configuration["MessageBroker:Host"],
                    UserName = configuration["MessageBroker:UserName"],
                    Password = configuration["MessageBroker:Password"],
                    Consumers = (cfg) =>
                    {
                        // TODO: Consumers will add.
                        //cfg.AddConsumer<>();

                        return cfg;
                    },
                    Endpoints = (cfg) =>
                    {
                        // TODO: Receive Endpoints will add.
                        //cfg.ReceiveEndpoint();
                    }
                };
            });

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();

            services.AddSingleton<UserServiceConfig>(new UserServiceConfig {
                BaseUrl = configuration["ExternalUrls:UserService"]
            });

            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.AddCoreLogging(configuration);

            return builder;
        }
    }
}
