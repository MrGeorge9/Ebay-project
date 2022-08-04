namespace Ebay_project.Models.DTOs
{
    public class ErrorDto
    {
        public string Error { get; }

        public ErrorDto(string error)
        {
            Error = error;
        }
    }
}
