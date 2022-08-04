using Ebay_project.Context;

namespace Ebay_project.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationContext _db;

        public AdminService(ApplicationContext db)
        {
            _db = db;
        }


        public string FillWalletOfUser(int userId, int ammount)
        {
            if (ammount <= 0)
            {
                return "Ammount has to be higher than 0";
            }
            
            var user = _db.Users.FirstOrDefault(p => p.Id == userId);
            if (user == null)
            {
                return "No such user";
            }

            user.Wallet += ammount;
            _db.SaveChanges();
            return "User wallet has been filled";
        }

        public string DeleteUser(int userId)
        {
            var user = _db.Users.FirstOrDefault(p => p.Id == userId);
            if (user == null)
            {
                return "No such user";
            }

            _db.Users.Remove(user);
            _db.SaveChanges();
            return "User has been removed";
        }
    }
}
