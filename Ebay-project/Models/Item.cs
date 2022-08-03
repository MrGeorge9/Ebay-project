namespace Ebay_project.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoURL { get; set; }
        public int StartPrice { get; set; }        
        public int PurchasePrice { get; set; }
        public bool Sold { get; set; }
        public string BuyersName { get; set; }

        public List<Bid> Bids { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public Item()
        {            
            Sold = false;
            Bids = new List<Bid>();
            BuyersName = string.Empty;
        }
    }
}
