using Core.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using User.DAL.Models;

namespace User.BL.Features.Commands
{
    public static class CreateUser
    {
        public class Command : IRequest<BaseResult>
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly UserManager<AppUser> _userManager;

            public CommandHandler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var appUser = new AppUser()
                {
                    UserName = request.UserName,
                    Email = request.Email,
                };

                PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();
                appUser.PasswordHash = ph.HashPassword(appUser, request.Password);

                var result = await _userManager.CreateAsync(appUser);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, AppRoles.User.ToString());

                    return BaseResult.Success();
                }

                var error = result.Errors.FirstOrDefault();
                return BaseResult.Failure(error?.Code, error?.Description);
            }
        }
    }
}
