using Core.Application.Common;
using Core.Application.Services;
using Newtonsoft.Json;
using System.Text;

namespace Core.Infrastructure.Services
{
    public class HttpCaller : IHttpCaller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpCaller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> Get<T>(Action<HttpCallOptions> options) where T : BaseHttpResult
        {
            var httpCallOptions = new HttpCallOptions();
            options(httpCallOptions);

            var requestUri = string.Empty;

            if (httpCallOptions.QueryStringElements != null || httpCallOptions.QueryStringElements.Count > 0)
                requestUri = CreateQueryString(httpCallOptions.BaseUrl, httpCallOptions.QueryStringElements);
            else
                requestUri = httpCallOptions.BaseUrl;

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var httpClient = _httpClientFactory.CreateClient();

            var baseHttpResult = new BaseHttpResult();

            try
            {
                var response = await httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    baseHttpResult = JsonConvert.DeserializeObject<T>(responseStream);

                    baseHttpResult.Succeeded = true;
                }

                baseHttpResult.StatusCode = response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                baseHttpResult.Succeeded = false;
            }

            return (T)baseHttpResult;
        }

        private string CreateQueryString(string baseUrl, List<string> queryStringList)
        {
            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append(baseUrl);
            
            for (int index = 0; index < queryStringList.Count; index++)
            {
                if (index == 0)
                    queryStringBuilder.Append("?");

                queryStringBuilder.Append(queryStringList[index]);

                if (index != queryStringList.Count - 1)
                    queryStringBuilder.Append("&");
            }

            return queryStringBuilder.ToString();
        }

        public TResponse Post<TRequest, TResponse>(Action<HttpCallOptions<TRequest>> options)
        {
            throw new NotImplementedException();
        }
    }
}
