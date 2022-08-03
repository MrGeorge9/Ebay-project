namespace Ebay_project.Models.DTOs
{
    public class ListAvailableDtoResponse
    {
        public string Name { get; }
        public string PhotoUrl { get; }
        public int LastBid { get; set; }

        public ListAvailableDtoResponse(string name)
        {
            Name = name;
        }

        public ListAvailableDtoResponse(string name, string photoUrl, int lastBid)
        {
            Name = name;
            PhotoUrl = photoUrl;
            LastBid = lastBid;
        }
    }
}
