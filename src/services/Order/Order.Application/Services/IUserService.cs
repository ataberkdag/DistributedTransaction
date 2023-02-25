using Order.Application.Models.Contracts;

namespace Order.Application.Services
{
    public interface IUserService
    {
        public Task<CheckUserResult> CheckUser(CheckUserRequest request);
    }
}
