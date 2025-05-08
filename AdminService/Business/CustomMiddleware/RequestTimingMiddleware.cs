using System.Diagnostics;
using Azure.Core;
using Microsoft.AspNetCore.Http;

namespace AdminService.Business.CustomMiddleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew(); // Bắt đầu đo thời gian

            await _next(context); // Gọi middleware tiếp theo trong pipeline

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            _logger.LogInformation($"Request to {context.Request.Path} took {elapsedMs} ms.");
        }

        //RequestDelegate _next: đại diện cho middleware tiếp theo.

        //InvokeAsync(): là phương thức bắt buộc để custom middleware.

        //Stopwatch: đo thời gian request.

        //ILogger: ghi log thời gian xử lý request.
    }
}
