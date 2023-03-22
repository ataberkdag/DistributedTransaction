using Core.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using User.BL.Models;
using User.BL.Services.Impl;
using User.BL.Services.Interfaces;

namespace User.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBL(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
            services.AddScoped<IDbContextHandler, DbContextHandler>();

            services.AddHttpContextAccessor();

            services.AddMediatR(x => {
                x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddJWT();

            // TODO: Consul implementation
            services.AddCoreInfrastructure(opt => {
                opt.EnableServiceRegistry = true;
                opt.ServiceRegistryOptions = configuration.GetSection("ServiceRegistry");
            });

            return services;
        }

        public static IServiceCollection AddJWT(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            var tokenOptions = sp.GetService<IOptions<TokenOptions>>().Value;
            byte[] secret = Encoding.UTF8.GetBytes(tokenOptions.SecretKey);

            services.AddAuthentication(options => { 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt => {
                opt.Audience = tokenOptions.Audience;
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IAccessTokenService, AccessTokenService>();

            return services;
        }
    }
}
