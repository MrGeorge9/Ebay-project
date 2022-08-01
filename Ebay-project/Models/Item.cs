namespace Ebay_project.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoURL { get; set; }
        public int Price { get; set; }

        public User User { get; set; }  
    }
}
