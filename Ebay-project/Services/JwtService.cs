using Ebay_project.Context;
using Ebay_project.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ebay_project.Services
{
    public class JwtService : IAuthService
    {
        private readonly ApplicationContext _db;
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public JwtService(ApplicationContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }


        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TokenGenerationKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User ReturnUserFromToken(IEnumerable<Claim> userClaims)
        {
            var userId = Int32.Parse(userClaims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = _db.Users.FirstOrDefault(p => p.Id == userId);

            return user;
        }
    }
}
