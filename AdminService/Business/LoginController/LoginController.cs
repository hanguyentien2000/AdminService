using AdminService.Business.Jwt;
using AdminService.Business.Jwt.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DataUtils;
namespace AdminService.Business.LoginController
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtHandler _jwtService;
        private readonly ITokenStoreHandler _tokenStore;

        public AuthController(IJwtHandler jwtService, ITokenStoreHandler tokenStore)
        {
            _jwtService = jwtService;
            _tokenStore = tokenStore;
        }

        [HttpPost("login")]
        public async Task<Response<object>> Login([FromBody] LoginModel request)
        {
            // Giả định kiểm tra thông tin tài khoản
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtService.GenerateToken("user-id-123", "Admin");
                _tokenStore.SaveToken("user-id-123", token);

                return Response<object>.Ok(new { token }, "Đăng nhập thành công");
            }

            return Response<object>.Fail("Tài khoản hoặc mật khẩu không đúng");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<Response<object>> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                _tokenStore.RevokeToken(userId);
                return Response<object>.Ok(null, "Đăng xuất thành công");
            }

            return Response<object>.Fail("Không xác định được người dùng");
        }
    }

}
