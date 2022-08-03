namespace Ebay_project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Wallet { get; set; }
        public string Role { get; set; }
        public List<Item> Items { get; set; }

        public User()
        {
            Wallet = 0;
            Role = "Buyer";
            Items = new List<Item>();
        }
    }
}
