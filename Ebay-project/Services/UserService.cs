using Ebay_project.Context;
using Ebay_project.Models;
using Ebay_project.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ebay_project.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _db;
        private readonly IAuthService _authService;

        public UserService(ApplicationContext db, IAuthService authService)
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

        public User ReadUser(IEnumerable<Claim> userClaims)
        {
            var userId = Int32.Parse(userClaims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = _db.Users.FirstOrDefault(p => p.Id == userId);

            return user;
        }

        public string CreateItem(User user, NewItemDto newItem)
        {
            if (newItem.Name == string.Empty && newItem.Description == string.Empty && newItem.PhotoUrl == string.Empty
                && newItem.StartingPrice == 0 && newItem.PurchasePrice == 0)
            {
                return "No data provided";
            }
            if (newItem.Name == string.Empty)
            {
                return "No name provided";
            }
            if (newItem.Description == string.Empty)
            {
                return "No description provided";
            }
            if (newItem.PhotoUrl == string.Empty)
            {
                return "No photoURL provided";
            }
            if (newItem.StartingPrice <= 0)
            {
                return "Starting price can´t be less than 1";
            }
            if (newItem.PurchasePrice <= 0)
            {
                return "Purchase price can´t be less than 1";
            }
            if (!(newItem.PhotoUrl.EndsWith("jpg") || newItem.PhotoUrl.EndsWith("jpeg") || newItem.PhotoUrl.EndsWith("png")))
            {
                return "Photo URL is incorrect";
            }

            Item item = new Item()
            {
                Name = newItem.Name,
                Description = newItem.Description,
                PhotoURL = newItem.PhotoUrl,
                StartPrice = newItem.StartingPrice,
                PurchasePrice = newItem.PurchasePrice
            };
           
            _db.Items.Add(item);
            user.Items.Add(item);
            _db.SaveChanges();

            return "Item is created";
        }
    }
}
