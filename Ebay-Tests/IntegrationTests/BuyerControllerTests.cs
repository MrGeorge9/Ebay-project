using System.Net.Http.Json;
using System.Text.Json;

namespace Ebay_Tests.IntegrationTests
{
    [Serializable]
    [Collection("Serialize")]
    public class BuyerControllerTests : IntegrationTests
    {
        private User user;

        public BuyerControllerTests()
        {
            this.user = new User()
            {
                Name = "George",
                Password = "Uhorka",
                Wallet = 1500,
                Role = "Admin"
            };
        }

        [Fact]
        public async Task ListAvailableItems_WithRightData_ReturnsAvailableItems()
        {            
            var token = authService.GenerateToken(user);          

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");           

            var response = await _client.PostAsync("api/list", JsonContent.Create(new ListAvailableDto(1)));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<List<Dictionary<string, Object>>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(2, result.Count());
            Assert.Equal("Phone", result[0]["name"].ToString());
        }

        [Fact]
        public async Task ListAvailableItems_WithWrongPage_ReturnsWrongPage()
        {          
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.PostAsync("api/list", JsonContent.Create(new ListAvailableDto(0)));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);          
            Assert.Equal("Selected page must be higher than 0", result["error"]);
        }

        [Fact]
        public async Task ListAvailableItems_WithEmptyPage_ReturnsEmptyPage()
        {
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.PostAsync("api/list", JsonContent.Create(new ListAvailableDto(5)));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("This page is empty", result["message"]);
        }

        [Fact]
        public async Task ListAvailableItems_WithoutAuthentication_ReturnsUnAuthorized()
        {
            var response = await _client.PostAsync("api/list", null);

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ItemDetails_WithRightData_ReturnsItemDetails()
        {
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("id", "1");

            var response = await _client.PostAsync("api/details", null);
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, Object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);            
            Assert.Equal("Phone", result["name"].ToString());
            Assert.Equal("Brand new phone", result["description"].ToString());
            Assert.Equal("1500", result["purchasePrice"].ToString());
        }

        [Fact]
        public async Task ItemDetails_WithWrongItem_ReturnsNoSuchItem()
        {
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("id", "5");

            var response = await _client.PostAsync("api/details", null);
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("No such item in the database", result["error"]);
        }

        [Fact]
        public async Task ItemDetails_WithoutAuthentication_ReturnsUnAuthorized()
        {
            var response = await _client.PostAsync("api/details", null);

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
