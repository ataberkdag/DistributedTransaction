using Core.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace User.BL.Features.Commands
{
    public static class CreateRole
    {
        public class Command : IRequest<BaseResult>
        {
            public string RoleName { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, BaseResult>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public CommandHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentRole = await _roleManager.FindByNameAsync(request.RoleName);
                if (currentRole is not null)
                    return BaseResult.Failure("1111", "Role already exists.");

                var role = new IdentityRole()
                {
                    Name = request.RoleName
                };

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                    return BaseResult.Success();

                var error = result.Errors.FirstOrDefault();
                return BaseResult.Failure(error?.Code, error?.Description);
            }
        }
    }
}
