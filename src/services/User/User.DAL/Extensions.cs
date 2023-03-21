using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.DAL.Models;

namespace User.DAL
{
    public static class Extensions
    {
        public static ModelBuilder SeedData(this ModelBuilder builder)
        {
            string ADMIN_ID = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            string USER_ID = "42c88e5a-700e-47a1-9912-1116c664a9ef";
            string ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                Id = USER_ID,
                ConcurrencyStamp = USER_ID
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            var appUser = new AppUser
            {
                Id = ADMIN_ID,
                Email = "admin@test.com",
                EmailConfirmed = true,
                UserName = "Admin",
                NormalizedEmail = "ADMIN@TEST.COM",
                NormalizedUserName = "ADMIN"
            };

            PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "admin_12345.");

            //seed user
            builder.Entity<AppUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            return builder;
        }
    }
}
