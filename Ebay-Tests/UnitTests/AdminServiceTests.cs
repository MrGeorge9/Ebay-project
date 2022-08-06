namespace Ebay_Tests.UnitTests
{
    [Serializable]
    [Collection("Serialize")]
    public class AdminServiceTests
    {
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "EbayTests").Options;
        private ApplicationContext _db;
        private AdminService _adminService;

        public AdminServiceTests()
        {
            _db = new ApplicationContext(options);
            _adminService = new AdminService(_db);
        }

        [Fact]
        public void FillWalletOfUserTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);
            Assert.Equal("Ammount has to be higher than 0", _adminService.FillWalletOfUser(1, 0));
            Assert.Equal("No such user", _adminService.FillWalletOfUser(4, 500));
            Assert.Equal("User wallet has been filled", _adminService.FillWalletOfUser(1, 500));
        }

        [Fact]
        public void DeleteUserTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);
            Assert.Equal("No such user", _adminService.DeleteUser(4));
            Assert.Equal("User has been removed", _adminService.DeleteUser(1));        
        }        
    }
}