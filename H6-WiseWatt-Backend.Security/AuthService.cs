using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Security.Interfaces;
using H6_WiseWatt_Backend.Security.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace H6_WiseWatt_Backend.Security
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJSonWebToken(UserEntity user)
        {
            var model = GetJwtSettings();

            var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Firstname),
                    new Claim(ClaimTypes.Email, user.Email),
                };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(model.TokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: model.TokenIssuer,
                audience: model.TokenAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(model.TokenExpirationMinutes),
                signingCredentials: creds
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            return jwtTokenHandler.WriteToken(token);
        }

        private JwtSettingEntity GetJwtSettings()
        {
            return _config.GetSection("JWT").Get<JwtSettingEntity>();
        }
    }
}
