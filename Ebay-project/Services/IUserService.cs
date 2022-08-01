using Ebay_project.Models;
using Ebay_project.Models.DTOs;

namespace Ebay_project.Services
{
    public interface IUserService
    {
        string Login(UserLogin userLogin);     
        User Authenticate(UserLogin userLogin);     
        string GenerateToken(User user);        
    }
}
