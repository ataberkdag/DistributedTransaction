using Newtonsoft.Json;
using Order.Application.Models;
using Order.Application.Models.Contracts;
using Order.Application.Services;
using System.Text.Json;

namespace Order.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserServiceConfig _config;

        public UserService(IHttpClientFactory httpClientFactory, UserServiceConfig config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<CheckUserResult> CheckUser(CheckUserRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_config.BaseUrl}?UserId={request.UserId}");

            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var httpClient = _httpClientFactory.CreateClient();

            var result = new CheckUserResult();

            var response = await httpClient.SendAsync(requestMessage);

            result.StatusCode = response.StatusCode.ToString();

            if (!response.IsSuccessStatusCode)
            {
                result.Succeeded = false;

                return result;
            }

            // TODO: Does not deserialize
            var responseStream = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<CheckUserResult>(responseStream);

            result.Succeeded = true;

            return result;

        }
    }
}
