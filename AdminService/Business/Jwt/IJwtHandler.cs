using System.Security.Claims;

namespace AdminService.Business.Jwt
{
    public interface IJwtHandler
    {
        string GenerateToken(string userId, string role);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
