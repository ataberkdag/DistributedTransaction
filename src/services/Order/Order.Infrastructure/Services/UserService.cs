using Core.Application.Services;
using Order.Application.Models;
using Order.Application.Models.Contracts;
using Order.Application.Services;

namespace Order.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserServiceConfig _config;
        private readonly IHttpCaller _httpCaller;

        public UserService(UserServiceConfig config, IHttpCaller httpCaller)
        {
            _config = config;
            _httpCaller = httpCaller;
        }

        public async Task<CheckUserResult> CheckUser(CheckUserRequest request)
        {
            var result = await _httpCaller.Get<CheckUserResult>(opt => {
                opt.BaseUrl = _config.BaseUrl;
                opt.QueryStringElements = new List<string> { $"UserId={request.UserId}" };
            });

            return result;
        }
    }
}
