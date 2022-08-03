namespace Ebay_project.Models.DTOs
{
    public class LoginResponseDto
    {
        public string JWTToken { get; }
        public int Wallet { get; }

        public LoginResponseDto(string jWTToken, int wallet)
        {
            JWTToken = jWTToken;
            Wallet = wallet;
        }
    }
}
