using Core.Application.Exceptions;
using Core.Application.Services;
using Order.Application;
using Order.Application.Models;
using Order.Application.Models.Contracts;
using Order.Application.Services;

namespace Order.Infrastructure.Services
{
    public class LimitService : ILimitService
    {
        private readonly LimitServiceConfig _config;
        private readonly IHttpCaller _httpCaller;

        public LimitService(LimitServiceConfig config, IHttpCaller httpCaller)
        {
            _config = config;
            _httpCaller = httpCaller;
        }

        public async Task<CheckLimitResult> CheckLimit(CheckLimitRequest request)
        {
            var result = await _httpCaller.Get<CheckLimitResult>(opt => {
                opt.BaseUrl = _config.BaseUrl;
                opt.QueryStringElements = new List<string> { $"UserId={request.UserId}" };
            });

            if (!result.Succeeded)
                throw new BusinessException(BusinessExceptionCodes.LimitServiceIntegrationError.GetHashCode().ToString(), BusinessExceptionCodes.LimitServiceIntegrationError.ToString());

            if (result.IsLimitExceeded)
                throw new BusinessException(BusinessExceptionCodes.LimitExceeded.GetHashCode().ToString(), BusinessExceptionCodes.LimitExceeded.ToString());

            return result;
        }
    }
}
