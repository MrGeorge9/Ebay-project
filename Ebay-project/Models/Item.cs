namespace Ebay_project.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoURL { get; set; }
        public int StartPrice { get; set; }
        public int Bid { get; set; }
        public int PurchasePrice { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public Item()
        {
            Bid = 0;
        }
    }
}
