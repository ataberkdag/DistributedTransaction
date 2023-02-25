using Order.Application.Models;
using Order.Application.Services;
using System.Text.Json;

namespace Order.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CheckUserResult> CheckUser(CheckUserRequest request)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5001/api/users/{request.UserId}");

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

            result.Succeeded = true;

            using var responseStream = await response.Content.ReadAsStreamAsync();
            result = await JsonSerializer.DeserializeAsync<CheckUserResult>(responseStream);

            return result;

        }
    }
}
