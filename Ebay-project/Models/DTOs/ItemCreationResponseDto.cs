namespace Ebay_project.Models.DTOs
{
    public class ItemCreationResponseDto
    {
        public string Message { get; }
        public NewItemDto NewItem { get; }

        public ItemCreationResponseDto(string message, NewItemDto newItemDto)
        {
            Message = message;
            NewItem = newItemDto;
        }
    }
}
