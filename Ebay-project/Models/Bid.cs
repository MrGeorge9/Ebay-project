namespace Ebay_project.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public Item Item { get; set; }
        public int ItemId { get; set; }
    }
}
