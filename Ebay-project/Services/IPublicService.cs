using Ebay_project.Models.DTOs;

namespace Ebay_project.Services
{
    public interface IPublicService
    {
        string Register(UserRegistrationDto userRegistration);
        string Login(UserLoginDto userLogin);
    }
}