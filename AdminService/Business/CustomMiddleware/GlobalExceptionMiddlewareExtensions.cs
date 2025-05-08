namespace AdminService.Business.CustomMiddleware
{
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder application)
        {
            return application.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
