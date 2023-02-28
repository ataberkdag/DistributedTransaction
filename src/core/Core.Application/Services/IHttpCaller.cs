using Core.Application.Common;

namespace Core.Application.Services
{
    public interface IHttpCaller
    {
        Task<T> Get<T>(Action<HttpCallOptions> options) where T : BaseHttpResult;
    }

    public class HttpCallOptions
    {
        public string BaseUrl { get; set; }
        public List<string> QueryStringElements { get; set; }
    }

    public class HttpCallOptions<T> : HttpCallOptions
    {
        public T Body { get; set; }
    }
}