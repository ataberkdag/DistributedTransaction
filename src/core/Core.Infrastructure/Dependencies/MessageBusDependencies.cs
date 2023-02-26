﻿using Core.Infrastructure.DependencyModels;
using Core.Infrastructure.Services;
using Core.Infrastructure.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Dependencies
{
    public static class MessageBusDependencies
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, MessageBusOptions options)
        {
            services.AddMassTransit(x =>
            {
                options.Consumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options.Host, h => {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });

                    options.Endpoints(cfg);
                });
            });

            if (options.IsProducer)
                services.AddSingleton<IMassTransitHandler, MassTransitHandler>();

            return services;
        }
    }
}
