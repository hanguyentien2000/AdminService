﻿using AdminService.Insfrastructure.Databases;
using DataUtils;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AdminService.Business.Jwt.Token
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;

        }

        public string GenerateAccessToken(IdmUsers user, string roleName)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            // Tạo các claims cho user, bao gồm roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            if (!string.IsNullOrEmpty(roleName)) {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenExpiryMinutes"])),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public void GenerateRefreshToken(IdmUsers user, string ipAddress, string deviceInfo)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); // Generate random refresh token

            // Set refresh token and expiry date
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenExpiryDays"]));
            user.IsRefreshTokenRevoked = false;
        }
    }

}
