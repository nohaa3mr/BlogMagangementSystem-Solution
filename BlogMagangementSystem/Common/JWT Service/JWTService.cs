using BlogMagangementSystem.Common.Entities;
using BlogMagangementSystem.Common.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogMagangementSystem.Common.JWT_Service
{
    public class JWTService 
    {
        private readonly IConfiguration _configuration;
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> GetTokenAsync(string userName,string email, Role? role)
        {
            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role,(role.GetType()).ToString())
            };

            var authKeyString = _configuration["Jwt:Key"];
            if (authKeyString?.Length < 32)
            {
                throw new ArgumentOutOfRangeException("Jwt:Key", "The key size must be at least 256 bits (32 bytes).");
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKeyString));
            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(AuthClaims),
                Expires = DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
                SigningCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var Token = new JwtSecurityTokenHandler().CreateToken(TokenDescriptor);
            if (Token is null) throw new ArgumentNullException("Token", "Token is null");
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(Token));

        }
    }

}
