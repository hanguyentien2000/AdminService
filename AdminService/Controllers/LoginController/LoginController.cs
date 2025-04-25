using AdminService.Business.Jwt;
using AdminService.Business.Jwt.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DataUtils;
using System.Text;
using System;
using AdminService.Insfrastructure;
using AdminService.Business.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AdminService.Controllers.LoginController
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AdminDataContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AdminDataContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginRequest>>> Login([FromBody] LoginRequest request)
        {
            var user = _context.IdmUsers.FirstOrDefault(u => u.UserName == request.Username);
            if (user == null || user.PasswordSalt != HashPassword(request.Password))
                return Unauthorized();

            var accessToken = _tokenService.GenerateAccessToken(user);

            // Generate and save refresh token into user entity
            _tokenService.GenerateRefreshToken(user, HttpContext.Connection.RemoteIpAddress?.ToString(), "Browser");

            await _context.SaveChangesAsync();  // Save refresh token into user record

            return Ok(new { accessToken, refreshToken = user.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<Response<RefreshRequest>>> Refresh([FromBody] RefreshRequest request)
        {
            var user = await _context.IdmUsers.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null || user.IsRefreshTokenRevoked || user.RefreshTokenExpiryDate < DateTime.UtcNow)
                return Unauthorized();

            var accessToken = _tokenService.GenerateAccessToken(user);
            return Ok(new { accessToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
        {
            var user = await _context.IdmUsers.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user != null)
            {
                user.IsRefreshTokenRevoked = true;
                user.RefreshToken = null;
                user.RefreshTokenExpiryDate = null;
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Logged out" });
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var byteValue in bytes)
                {
                    builder.Append(byteValue.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    public record LoginRequest(string Username, string Password);
    public record RefreshRequest(string RefreshToken);
}
