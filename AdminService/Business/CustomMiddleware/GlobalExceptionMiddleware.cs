using System.Net;
using System.Text.Json;

namespace AdminService.Business.CustomMiddleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Chuyển tiếp request nếu không có lỗi
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception caught by middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        public static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                status = context.Response.StatusCode,
                error = "Internal Server Error",
                message = ex.Message
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
