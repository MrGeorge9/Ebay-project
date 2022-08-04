using Ebay_project.Models;
using System.Security.Claims;

namespace Ebay_project.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        User ReturnUserFromToken(IEnumerable<Claim> userClaims);
    }
}
