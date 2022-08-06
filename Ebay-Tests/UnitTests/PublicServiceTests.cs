using Moq;

namespace Ebay_Tests.UnitTests
{
    [Serializable]
    [Collection("Serialize")]
    public class PublicServiceTests
    {
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "EbayTests").Options;
        private ApplicationContext _db;
        private PublicService _publicService;

        public PublicServiceTests()
        {                       
            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(p => p.GenerateToken(It.IsAny<User>())).Returns("Absolutely perfect token");
            _db = new ApplicationContext(options);
            _publicService = new PublicService(_db,authServiceMock.Object);
        }

        [Fact]
        public void RegisterTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);

            var emptyRequest = new UserRegistrationDto(string.Empty, string.Empty);
            var emptyNameRequest = new UserRegistrationDto(string.Empty, "Uhorka");
            var emptyPasswordRequest = new UserRegistrationDto("John", string.Empty);
            var sameUserRequest = new UserRegistrationDto("George", "Uhorka");
            var shortPasswordRequest = new UserRegistrationDto("John", "Uhorka");
            var wrongPasswordRequest = new UserRegistrationDto("John", "UhorkaSalam");
            var RightRequest = new UserRegistrationDto("John", "UhorkaSalam1");

            Assert.Equal("No data provided", _publicService.Register(emptyRequest));
            Assert.Equal("No name provided", _publicService.Register(emptyNameRequest));
            Assert.Equal("No pasword provided", _publicService.Register(emptyPasswordRequest));
            Assert.Equal("Username already taken", _publicService.Register(sameUserRequest));
            Assert.Equal("Password must be at least 8 characters long", _publicService.Register(shortPasswordRequest));
            Assert.Equal("Password must contain at leat one special character", _publicService.Register(wrongPasswordRequest));
            Assert.Equal("New user has been created", _publicService.Register(RightRequest));
        }

        [Fact]
        public void LoginTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);

            var emptyRequest = new UserLoginDto(string.Empty, string.Empty);
            var emptyNameRequest = new UserLoginDto(string.Empty, "Uhorka");
            var emptyPasswordRequest = new UserLoginDto("George", string.Empty);
            var noUserRequest = new UserLoginDto("John", "Uhorka");
            var wrongPasswordRequest = new UserLoginDto("George", "Salama");
            var rightRequest = new UserLoginDto("George", "Uhorka");          

            Assert.Equal("No data provided", _publicService.Login(emptyRequest));
            Assert.Equal("No name provided", _publicService.Login(emptyNameRequest));
            Assert.Equal("No pasword provided", _publicService.Login(emptyPasswordRequest));
            Assert.Equal("No such user", _publicService.Login(noUserRequest));
            Assert.Equal("Password is incorrect", _publicService.Login(wrongPasswordRequest));
            Assert.Equal("Absolutely perfect token&1500", _publicService.Login(rightRequest));            
        }        
    }
}
