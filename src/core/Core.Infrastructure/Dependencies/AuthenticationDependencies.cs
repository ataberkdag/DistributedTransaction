using Core.Infrastructure.DependencyModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Core.Infrastructure.Dependencies
{
    public static class AuthenticationDependencies
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, SwaggerOptions swaggerOptions)
        {
            var sp = services.BuildServiceProvider();

            var tokenOptions = sp.GetService<IOptions<DependencyModels.TokenOptions>>().Value;
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

            if (swaggerOptions.EnableSwaggerGen)
            {
                services.AddSwaggerGen(c =>
                {
                    c.CustomSchemaIds(type => type.ToString());
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1", Description = "API" });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                        }
                    });

                    if (swaggerOptions is not null)
                    {
                        var xmlFilename = $"{swaggerOptions.ApiAssembly.GetName().Name}.xml";
                        c.IncludeXmlComments(Path.Combine(swaggerOptions.BaseDirectory, xmlFilename));
                    }

                });
            }

            return services;
        }
    }
}
