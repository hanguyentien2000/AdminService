using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace Utils.Middlewares
{
//    Middleware trong ASP.NET Core là các thành phần phần mềm nằm giữa request từ client và response từ server, đóng vai trò quan trọng trong việc xử lý các tác vụ chung trước khi request được xử lý bởi các API hoặc dịch vụ.

//Dưới đây là một số lý do tại sao bạn nên sử dụng middleware trong hệ thống MSA:

//Quản lý Request và Response Toàn Cục: Middleware cho phép bạn can thiệp vào pipeline xử lý của tất cả các request mà không cần phải lặp lại logic trong từng service.Điều này giúp tiết kiệm thời gian và công sức khi xử lý các vấn đề phổ biến như logging, xác thực người dùng(authentication), và phân quyền(authorization).

//Tính Tái Sử Dụng Cao: Các tính năng như logging, kiểm tra lỗi, và bảo mật có thể được tái sử dụng cho tất cả các dịch vụ mà không cần phải cài đặt lại logic cho mỗi service riêng biệt.

//Bảo Mật: Middleware có thể giúp bạn bảo vệ toàn bộ hệ thống bằng cách kiểm tra thông tin xác thực và phân quyền trong tất cả các request, giúp tránh được lỗ hổng bảo mật nếu không xử lý tốt các API riêng lẻ.

//Quản lý Lỗi và Thông Báo: Middleware có thể giúp bạn xử lý tất cả các ngoại lệ xảy ra trong toàn bộ pipeline một cách tập trung, giúp việc quản lý lỗi trở nên dễ dàng hơn.

//Khả Năng Mở Rộng: Khi hệ thống phát triển thêm nhiều dịch vụ, việc sử dụng middleware giúp dễ dàng thêm các tính năng mới mà không cần phải thay đổi từng service một.

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);  // Tiếp tục xử lý request
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về mã lỗi HTTP 500
                _logger.LogError(ex, "An unexpected error occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }
}
