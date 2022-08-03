namespace Ebay_project.Models.DTOs
{
    public class StatusDto
    {
        public string Message { get; }

        public StatusDto(string message)
        {
            Message = message;
        }
    }
}
