namespace PersonalPortfolio.Models
{
    public class ErrorViewModel
    {
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? RequestId { get; set; }
    }
}