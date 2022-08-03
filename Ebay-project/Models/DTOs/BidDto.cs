namespace Ebay_project.Models.DTOs
{
    public class BidDto
    {
        public int Value { get; }
        public string BidBy { get;}

        public BidDto(int value, string better)
        {
            Value = value;
            BidBy = better;
        }
    }
}
