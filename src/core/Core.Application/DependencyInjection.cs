using Core.Application.CrossCutting;
using Core.Application.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreApplication(this IServiceCollection services, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddMediatR(x => { 
                x.RegisterServicesFromAssembly(assembly);
            });

            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }

        public static IApplicationBuilder UseCoreApplication(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlingMiddleware>();
            builder.UseMiddleware<CorrelationMiddleware>();

            return builder;
        }
    }
}
