using Ebay_project.Context;
using Ebay_project.Models;
using Microsoft.IdentityModel.Tokens;

namespace Ebay_project.Exntensions
{
    public class Extensions
    {
        public static void MapSecretsToEnvVariables()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            foreach (var child in config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
        }

        public static void FillDatabaseIfEmpty(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (db.Users.IsNullOrEmpty())
            {
                User user1 = new User()
                {
                    Name = "George",
                    Password = "Uhorka",
                    Wallet = 1500,
                    Role = "Admin"
                };

                User user2 = new User()
                {
                    Name = "James",
                    Password = "Salama",
                    Wallet = 220,
                    Role = "Buyer"
                };

                Item item1 = new Item()
                {
                    Name = "Phone",
                    Description = "Brand new phone",
                    PhotoURL = "phone.jpg",
                    StartPrice = 350,
                    PurchasePrice = 1500,
                    Sold = false,
                };

                Item item2 = new Item()
                {
                    Name = "PC",
                    Description = "Brand new computer",
                    PhotoURL = "pc.jpg",
                    StartPrice = 820,
                    PurchasePrice = 2400,
                    Sold = false,
                };

                Item item3 = new Item()
                {
                    Name = "Cup",
                    Description = "Brand new cup",
                    PhotoURL = "cup.jpg",
                    StartPrice = 4,
                    PurchasePrice = 18,
                    Sold = true,
                    BuyersName = "James"
                };

                Bid bid1 = new Bid()
                {
                    Value = 480,
                };
                Bid bid2 = new Bid()
                {
                    Value = 580,
                };
                Bid bid3 = new Bid()
                {
                    Value = 1000,
                };
                Bid bid4 = new Bid()
                {
                    Value = 1500,
                };
                Bid bid5 = new Bid()
                {
                    Value = 7,
                };
                Bid bid6 = new Bid()
                {
                    Value = 13,
                };
                Bid bid7 = new Bid()
                {
                    Value = 19,
                };

                db.Users.AddRange(user1, user2);
                db.Items.AddRange(item1, item2, item3);
                db.Bids.AddRange(bid1, bid2, bid3, bid4, bid5, bid6, bid7);

                user1.Items.Add(item1);
                user1.Items.Add(item3);
                user2.Items.Add(item2);

                user1.Bids.Add(bid1);
                user1.Bids.Add(bid4);
                user2.Bids.Add(bid2);
                user2.Bids.Add(bid3);
                user2.Bids.Add(bid5);
                user1.Bids.Add(bid6);
                user2.Bids.Add(bid7);

                item1.Bids.Add(bid1);
                item1.Bids.Add(bid2);
                item2.Bids.Add(bid3);
                item2.Bids.Add(bid4);
                item3.Bids.Add(bid5);
                item3.Bids.Add(bid6);
                item3.Bids.Add(bid7);
                db.SaveChanges();
            }
        }
    }
}
