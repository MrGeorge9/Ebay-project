using System.Net.Http.Json;
using System.Text.Json;

namespace Ebay_Tests.IntegrationTests
{
    [Serializable]
    [Collection("Serialize")]
    public class PublicControllerTests : IntegrationTests
    {

        [Fact]
        public async Task Register_WithRightData_ReturnsUserCreated()
        {            
            UserRegistrationDto userRegistrationDto = new UserRegistrationDto("John", "Salama123");

            var response = await _client.PostAsync("api/register", JsonContent.Create(userRegistrationDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("New user has been created", result["message"]);
        }

        [Fact]
        public async Task Register_WithNoName_ReturnsNoName()
        {
            UserRegistrationDto userRegistrationDto = new UserRegistrationDto(string.Empty, "Salama123");

            var response = await _client.PostAsync("api/register", JsonContent.Create(userRegistrationDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("No name provided", result["error"]);
        }

        [Fact]
        public async Task Register_WithShortPassword_ReturnsShortPassword()
        {
            UserRegistrationDto userRegistrationDto = new UserRegistrationDto("John", "Salama");

            var response = await _client.PostAsync("api/register", JsonContent.Create(userRegistrationDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Password must be at least 8 characters long", result["error"]);
        }

        [Fact]
        public async Task Login_WithRightData_ReturnsUserLoggedin()
        {          
            UserLoginDto userLoginDto = new UserLoginDto("George", "Uhorka");

            var response = await _client.PostAsync("api/login", JsonContent.Create(userLoginDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("1500", result["wallet"].ToString());
        }

        [Fact]
        public async Task Login_WithNoPassword_ReturnsNoPassword()
        {
            UserLoginDto userLoginDto = new UserLoginDto("George", string.Empty);

            var response = await _client.PostAsync("api/login", JsonContent.Create(userLoginDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("No pasword provided", result["error"]);
        }

        [Fact]
        public async Task Login_WithNotExistingUser_ReturnsNoSuchUser()
        {
            UserLoginDto userLoginDto = new UserLoginDto("Johny", "Salamka");

            var response = await _client.PostAsync("api/login", JsonContent.Create(userLoginDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("No such user", result["error"]);
        }
    }
}
