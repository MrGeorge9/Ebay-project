using Ebay_project.Models;

namespace Ebay_project.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}
