using Ebay_project.Context;
using Ebay_project.Models;
using Ebay_project.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ebay_project.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _db;

        public UserService(ApplicationContext db)
        {
            _db = db;
        }

        public string Login(UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            
            if (user != null)
            {
                return GenerateToken(user);                
            }

            return string.Empty;
        }

        public User Authenticate(UserLogin userLogin)
        {
            var currentUser = _db.Users.FirstOrDefault(p => p.Name.ToLower().Equals(userLogin.Name.ToLower()) && 
                p.Password.Equals(userLogin.Password));

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TokenGenerationKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
