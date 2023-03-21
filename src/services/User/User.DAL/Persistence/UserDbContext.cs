using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.DAL.Models;

namespace User.DAL.Persistence
{
    public class UserDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUserToken> AppUserTokens { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.SeedData();
        }
    }
}
