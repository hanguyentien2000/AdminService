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
using AdminService.Insfrastructure.Databases;
using Azure.Core;

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

        [Authorize]
        [HttpGet("")]
        public IActionResult SomeAction()
        {
            if (User.IsInRole("Administrator"))
            {
                return Ok("You have administrator access.");
            }
            else
            {
                return Forbid();  // Trả về 403 nếu không có quyền
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginRequest>>> Login([FromBody] LoginRequest request)
        {
            var user = await _context.IdmUsers.FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user != null)
            {
                var uio = await _context.IdmUsersInRoles.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                var checkRole = await _context.IdmRoles.FirstOrDefaultAsync(u => u.RoleId == uio.RoleId);
                if ((user.Password != null || user.Password != "") && (user.PasswordSalt == null || user.PasswordSalt == ""))
                    user.PasswordSalt = HashPassword(user.Password);

                if (user == null || user.PasswordSalt != HashPassword(request.Password))
                    return Unauthorized();

                var accessToken = _tokenService.GenerateAccessToken(user, checkRole.RoleName);

                // Generate and save refresh token into user entity
                _tokenService.GenerateRefreshToken(user, HttpContext.Connection.RemoteIpAddress?.ToString(), "Browser");

                await _context.SaveChangesAsync();  // Save refresh token into user record
                return Ok(new { accessToken, refreshToken = user.RefreshToken });
            }
            else
                return BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<Response<RefreshRequest>>> Refresh([FromBody] RefreshRequest request)
        {
            var user = await _context.IdmUsers.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.IsRefreshTokenRevoked || user.RefreshTokenExpiryDate < DateTime.UtcNow)
                return Unauthorized();
            else
            {
                var uio = await _context.IdmUsersInRoles.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                if (uio != null)
                {
                    var checkRole = await _context.IdmRoles.FirstOrDefaultAsync(u => u.RoleId == uio.RoleId);

                    var accessToken = _tokenService.GenerateAccessToken(user, checkRole.RoleName);
                    return Ok(new { accessToken });
                }
                else
                    return BadRequest();
            }
        }

        [HttpPost("logout")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
        {
            var user = await _context.IdmUsers.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user != null)
            {
                user.IsRefreshTokenRevoked = true;
                user.RefreshToken = "";
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
