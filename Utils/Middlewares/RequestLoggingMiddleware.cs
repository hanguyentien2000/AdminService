using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Utils.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log thông tin request
            _logger.LogInformation("Request: {method} {url}", context.Request.Method, context.Request.Path);

            // Tiếp tục xử lý request
            await _next(context);
        }
    }
}
