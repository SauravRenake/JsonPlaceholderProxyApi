using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace JsonPlaceholderProxyApi.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Check if request already has correlation id
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            // 2. Store correlation id in HttpContext
            context.Items[CorrelationIdHeader] = correlationId;

            // 3. Add correlation id to response headers
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });

            // 4. Call next middleware
            await _next(context);
        }
    }
}
