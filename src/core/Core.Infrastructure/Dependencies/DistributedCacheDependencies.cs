﻿using Core.Application.Services;
using Core.Infrastructure.Models;
using Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
namespace Core.Infrastructure.Dependencies
{
    public static class DistributedCacheDependencies
    {
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, DistributedCacheOptions options)
        {
            services.AddDistributedCache(new DistributedCacheOptions { 
                Endpoints = options.Endpoints,
                Password = options.Password,
                Database = options.Database
            });

            services.AddStackExchangeRedisCache(setup =>
            {
                setup.ConfigurationOptions = new ConfigurationOptions
                {
                    EndPoints = {
                        options.Endpoints
                    },
                    Password = options.Password,
                    DefaultDatabase = options.Database
                };
            });

            services.AddSingleton<ICacheProvider, CacheProvider>();

            return services;
        }
    }
}
