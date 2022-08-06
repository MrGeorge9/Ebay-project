using System.Text.Json;


namespace Ebay_Tests.IntegrationTests
{
    [Serializable]
    [Collection("Serialize")]
    public class AdminControllerTests : IntegrationTests
    {
        private User user;

        public AdminControllerTests()
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
        public async Task FillWalletOfAUser_WithRightData_ReturnsWalletFilled()
        {            
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("userId", "2");
            _client.DefaultRequestHeaders.Add("ammount", "500");           

            var response = await _client.PostAsync("api/fill", null);
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("User wallet has been filled", result["message"]);
        }

        [Fact]
        public async Task FillWalletOfAUser_WithWrongAmmount_ReturnsWrongAmmount()
        {           
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("userId", "2");
            _client.DefaultRequestHeaders.Add("ammount", "0");

            var response = await _client.PostAsync("api/fill", null);
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Ammount has to be higher than 0", result["error"]);
        }

        [Fact]
        public async Task FillWalletOfAUser_WithWrongRole_ReturnsForbidden()
        {
            user.Role = "Buyer";
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("userId", "2");
            _client.DefaultRequestHeaders.Add("ammount", "0");

            var response = await _client.PostAsync("api/fill", null);            

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);            
        }

        [Fact]
        public async Task FillWalletOfAUser_WithoutAuthentication_ReturnsUnAuthorized()
        {          
            var response = await _client.PostAsync("api/fill", null);

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_WithRightData_ReturnsUserDeleted()
        {            
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("userId", "2");            

            var response = await _client.PostAsync("api/delete", null);
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("User has been removed", result["message"]);
        }

        [Fact]
        public async Task DeleteUser_WithWrongUser_ReturnsNoSuchUser()
        {           
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("userId", "5");

            var response = await _client.PostAsync("api/delete", null);
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("No such user", result["error"]);
        }

        [Fact]
        public async Task DeleteUser_WithWrongRole_ReturnsForbidden()
        {
            user.Role = "Buyer";
            var token = authService.GenerateToken(user);

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            _client.DefaultRequestHeaders.Add("userId", "2");            

            var response = await _client.PostAsync("api/delete", null);

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_WithoutAuthentication_ReturnsUnAuthorized()
        {
            var response = await _client.PostAsync("api/delete", null);

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
