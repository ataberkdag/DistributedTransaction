using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.DAL.Models;
using User.DAL.Persistence;

namespace User.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services.AddDbContext<UserDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("UserDb")));

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();

            services.Configure<IdentityOptions>(opt => {
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 5;
            });

            return services;
        }
    }
}
