using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ebay_Tests.UnitTests
{
    [Serializable]
    [Collection("Serialize")]
    public class JwtServiceTests
    {
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "EbayTests").Options;
        private ApplicationContext _db;
        private JwtService _jwtService;

        public JwtServiceTests()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            _db = new ApplicationContext(options);
            _jwtService = new JwtService(_db, config);
        }  

        [Fact]
        public void GenerateTokenTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);

            var user = _db.Users.FirstOrDefault();           
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

            var tokenString =  new JwtSecurityTokenHandler().WriteToken(token);

            Assert.Equal(tokenString, _jwtService.GenerateToken(user));       
        }                
    }
}
