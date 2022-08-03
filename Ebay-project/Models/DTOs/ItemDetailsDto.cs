namespace Ebay_project.Models.DTOs
{
    public class ItemDetailsDto
    {
        public string Name { get; }
        public string Description { get; }
        public string PhotoUrl { get; }
        public List<BidDto> Bids { get; }
        public int PurchasePrice { get; }
        public string SellersName { get; }
        public string BuyersName { get; }

        public ItemDetailsDto(string name)
        {
            Name = name;
        }

        public ItemDetailsDto(string name, string description, string photoUrl, List<BidDto> bids, int purchasePrice, string sellersName, string buyersName)
        {
            Name = name;
            Description = description;
            PhotoUrl = photoUrl;
            Bids = bids;
            PurchasePrice = purchasePrice;
            SellersName = sellersName;
            BuyersName = buyersName;
        }
    }
}
