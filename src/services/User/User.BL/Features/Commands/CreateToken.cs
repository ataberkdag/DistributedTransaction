using Core.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using User.BL.Services.Interfaces;
using User.DAL.Models;

namespace User.BL.Features.Commands
{
    public static class CreateToken
    {
        public class Command : IRequest<BaseResult<Response>>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult<Response>>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IAccessTokenService _accessTokenGenerator;

            public CommandHandler(UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IAccessTokenService accessTokenGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _accessTokenGenerator = accessTokenGenerator;
            }

            public async Task<BaseResult<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                    return BaseResult<Response>.Failure("9999", "User Not Found");

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

                if (!result.Succeeded)
                    return BaseResult<Response>.Failure("9999", "Login Failed");

                var userRoles = (await _userManager.GetRolesAsync(user))?.ToList();

                var userToken = await _accessTokenGenerator.GetToken(user, userRoles);

                return BaseResult<Response>.Success(new Response
                {
                    Token = userToken.Value,
                    ExpireDate = userToken.ExpireDate
                });
            }
        }

        public class Response
        {
            public string Token { get; set; }
            public DateTime ExpireDate { get; set; }
        }
    }
}
