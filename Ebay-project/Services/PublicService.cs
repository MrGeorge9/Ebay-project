using Ebay_project.Context;
using Ebay_project.Models;
using Ebay_project.Models.DTOs;

namespace Ebay_project.Services
{
    public class PublicService : IPublicService
    {
        private readonly ApplicationContext _db;
        private readonly IAuthService _authService;

        public PublicService(ApplicationContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public string Register(UserRegistrationDto userRegistration)
        {
            if (userRegistration.Name == string.Empty && userRegistration.Password == string.Empty)
            {
                return "No data provided";
            }
            if (userRegistration.Name == string.Empty)
            {
                return "No name provided";
            }
            if (userRegistration.Password == string.Empty)
            {
                return "No pasword provided";
            }
            if (_db.Users.Any(p => p.Name.Equals(userRegistration.Name)))
            {
                return "Username already taken";
            }
            if (userRegistration.Password.Length < 8)
            {
                return "Password must be at least 8 characters long";
            }
            if (userRegistration.Password.All(p => Char.IsLetter(p)))
            {
                return "Password must contain at leat one special character";
            }

            _db.Users.Add(new User() { Name = userRegistration.Name, Password = userRegistration.Password });
            _db.SaveChanges();

            return "New user has been created";
        }

        public string Login(UserLoginDto userLogin)
        {
            if (userLogin.Name == string.Empty && userLogin.Password == string.Empty)
            {
                return "No data provided";
            }
            if (userLogin.Name == string.Empty)
            {
                return "No name provided";
            }
            if (userLogin.Password == string.Empty)
            {
                return "No pasword provided";
            }

            var user = _db.Users.FirstOrDefault(p => p.Name.Equals(userLogin.Name));
            if (user == null)
            {
                return "No such user";
            }
            if (!userLogin.Password.Equals(user.Password))
            {
                return "Password is incorrect";
            }

            return _authService.GenerateToken(user) + "&" + user.Wallet;
        }


        public string DeleteItem(int id)
        {
            var bid = _db.Bids.FirstOrDefault(p => p.Id == id);

            _db.Bids.Remove(bid);
            _db.SaveChanges();
            return "Item deleted";
        }
    }
}
