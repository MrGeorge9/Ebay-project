namespace Ebay_project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public Item Item { get; set; }
        public int ItemId { get; set; }
    }
}
