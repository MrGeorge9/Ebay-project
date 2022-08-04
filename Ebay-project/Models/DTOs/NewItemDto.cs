namespace Ebay_project.Models.DTOs
{
    public class NewItemDto
    {
        public string Name { get; }
        public string Description { get; }
        public string PhotoUrl { get; }
        public int StartingPrice { get; }
        public int PurchasePrice { get; }

        public NewItemDto(string name, string description, string photoUrl, int startingPrice, int purchasePrice)
        {
            Name = name;
            Description = description;
            PhotoUrl = photoUrl;
            StartingPrice = startingPrice;
            PurchasePrice = purchasePrice;
        }
    }
}
