using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Middlewares
{
    public class CorrelationMiddleware
    {
        private RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CorrelationMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryAdd("CorrelationId", Guid.NewGuid().ToString("N"));

            await _next(context);
        }
    }
}
