using Application.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public string GenerateToken(string userId, string role, bool isRefreshToken)
        {
            // tạo key
            var secretKey = _configuration["Jwt:SecretKey"];
            var keyByte = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credential = new SigningCredentials(keyByte, SecurityAlgorithms.HmacSha256);

            // tạo claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var expiration = isRefreshToken ? _configuration["Jwt:ExpirationRF"] : _configuration["Jwt:ExpirationAT"];
            // tạo meta của token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer" ?? "tien"],
                audience: _configuration["Jwt:Audience" ?? "tien"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(expiration ?? "60")),
                signingCredentials: credential
             );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
