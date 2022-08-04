namespace Ebay_project.Services
{
    public interface IAdminService
    {
        string FillWalletOfUser(int userId, int ammount);
        string DeleteUser(int userId);
    }
}