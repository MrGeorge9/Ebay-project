namespace Ebay_Tests.UnitTests
{
    [Serializable]
    [Collection("Serialize")]
    public class BuyerServiceTests
    {
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
           .UseInMemoryDatabase(databaseName: "EbayTests").Options;
        private ApplicationContext _db;
        private BuyerService _buyerService;

        public BuyerServiceTests()
        {
            _db = new ApplicationContext(options);
            _buyerService = new BuyerService(_db);
        }

        [Fact]
        public void CreateItemTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);

            var user = _db.Users.FirstOrDefault();
            var emptyRequest = new NewItemDto(string.Empty, string.Empty, string.Empty, 0, 0);
            var noNameRequest = new NewItemDto(string.Empty, "Brand new phone", "coool.jpg", 10, 10);
            var noDescRequest = new NewItemDto("Phone", string.Empty, "coool.jpg", 10, 10);
            var noURLRequest = new NewItemDto("Phone", "Brand new phone", string.Empty, 10, 10);
            var wrongStartPriceRequest = new NewItemDto("Phone", "Brand new phone", "coool.jpg", 0, 10);
            var wrongPurchasePriceRequest = new NewItemDto("Phone", "Brand new phone", "coool.jpg", 10, 0);
            var RightRequest = new NewItemDto("Phone", "Brand new phone", "coool.jpg", 10, 10);
            
            Assert.Equal("No data provided", _buyerService.CreateItem(user, emptyRequest));
            Assert.Equal("No name provided", _buyerService.CreateItem(user, noNameRequest));
            Assert.Equal("No description provided", _buyerService.CreateItem(user, noDescRequest));
            Assert.Equal("No photoURL provided", _buyerService.CreateItem(user, noURLRequest));
            Assert.Equal("Starting price can´t be less than 1", _buyerService.CreateItem(user, wrongStartPriceRequest));
            Assert.Equal("Purchase price can´t be less than 1", _buyerService.CreateItem(user, wrongPurchasePriceRequest));
            Assert.Equal("Item is created", _buyerService.CreateItem(user, RightRequest));           
        }

        [Fact]
        public void ListAvailableItemsTest()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            for (int i = 0; i < 55; i++)
            {
                _db.Items.Add(new Item() { Name = "Item " + i, Description = "Great", PhotoURL = "cool.jpg"});
                _db.SaveChanges();
            }

            Assert.Equal("Selected page must be higher than 0", _buyerService.ListAvailableItems(new ListAvailableDto(0))[0].Name);
            Assert.Equal(20, _buyerService.ListAvailableItems(new ListAvailableDto(1)).Count);
            Assert.Equal(15, _buyerService.ListAvailableItems(new ListAvailableDto(3)).Count);
            Assert.Equal("This page is empty", _buyerService.ListAvailableItems(new ListAvailableDto(5))[0].Name);
            Assert.Equal("Item 20", _buyerService.ListAvailableItems(new ListAvailableDto(2))[0].Name);          
        }

        [Fact]
        public void ItemDetailsTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);

            List<BidDto> bidDtos = new List<BidDto>();
            var item = _db.Items.FirstOrDefault();
            
            ItemDetailsDto itemDetailsDto = new ItemDetailsDto(
                item.Name,
                item.Description,
                item.PhotoURL,
                bidDtos,
                item.PurchasePrice,
                item.User.Name,
                item.BuyersName
                );

            Assert.Equal("No such item in the database", _buyerService.ItemDetails(0).Name);
            Assert.Equal(itemDetailsDto.Name, _buyerService.ItemDetails(1).Name);
            Assert.Equal(itemDetailsDto.Description, _buyerService.ItemDetails(1).Description);
            Assert.Equal(itemDetailsDto.BuyersName, _buyerService.ItemDetails(1).BuyersName);
            Assert.Equal(itemDetailsDto.PurchasePrice, _buyerService.ItemDetails(1).PurchasePrice);          
        }

        [Fact]
        public void BidOnItemTest()
        {
            SeedDatabase.SeedDatabaseForTests(_db);

            List<BidDto> bidDtos = new List<BidDto>();
            var user1 = _db.Users.FirstOrDefault();
            var user2 = new User() { Name = "John", Password = "Johny123", Wallet = 0 };
            var user3 = _db.Users.FirstOrDefault(p => p.Id == 2);
            var item = _db.Items.FirstOrDefault();

            ItemDetailsDto itemDetailsDto = new ItemDetailsDto(
                item.Name,
                item.Description,
                item.PhotoURL,
                bidDtos,
                item.PurchasePrice,
                item.User.Name,
                item.BuyersName
                );

            Assert.Equal("No such item in the database", _buyerService.BidOnItem(user1, 5, 500).Name);
            Assert.Equal("User doesn´t have any money :(", _buyerService.BidOnItem(user2, 1, 500).Name);
            Assert.Equal("This item is already sold", _buyerService.BidOnItem(user1, 3, 500).Name);
            Assert.Equal("User has less money that the bid he wants to place", _buyerService.BidOnItem(user3, 1, 500).Name);
            Assert.Equal("The bid is lower than the last bid", _buyerService.BidOnItem(user1, 1, 500).Name);
            
            Assert.Equal(itemDetailsDto.Name, _buyerService.BidOnItem(user1, 1, 1000).Name);
            Assert.Equal(itemDetailsDto.Description, _buyerService.BidOnItem(user1, 1, 1000).Description);
            Assert.Equal(itemDetailsDto.BuyersName, _buyerService.BidOnItem(user1, 1, 1000).BuyersName);
            Assert.Equal(itemDetailsDto.PurchasePrice, _buyerService.BidOnItem(user1, 1, 1000).PurchasePrice);
        }        
    }
}
