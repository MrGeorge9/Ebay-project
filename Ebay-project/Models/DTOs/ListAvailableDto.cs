namespace Ebay_project.Models.DTOs
{
    public class ListAvailableDto
    {
        public int Page { get; }

        public ListAvailableDto(int page)
        {
            Page = page;
        }
    }
}
