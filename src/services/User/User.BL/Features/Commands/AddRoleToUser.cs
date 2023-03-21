using Core.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using User.BL.Services.Interfaces;
using User.DAL.Models;

namespace User.BL.Features.Commands
{
    public static class AddRoleToUser
    {
        public class Command : IRequest<BaseResult>
        {
            public Guid RoleId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IHttpContextAccessor _httpContext;
            private readonly IAccessTokenService _accessTokenGenerator;

            public CommandHandler(UserManager<AppUser> userManager,
                RoleManager<IdentityRole> roleManager,
                IHttpContextAccessor httpContext,
                IAccessTokenService accessTokenGenerator)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _httpContext = httpContext;
                _accessTokenGenerator = accessTokenGenerator;
            }
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var userId = _httpContext.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

                var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
                if (role is null)
                    return BaseResult.Failure("9999", "Role Not Found");

                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                    return BaseResult.Failure("9999", "User Not Found");

                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    await _accessTokenGenerator.DeleteToken(user);
                    return BaseResult.Success();
                }

                var error = result.Errors.FirstOrDefault();
                return BaseResult.Failure(error?.Code, error?.Description);
            }
        }
    }
}
