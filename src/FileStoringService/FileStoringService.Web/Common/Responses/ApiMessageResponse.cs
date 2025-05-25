namespace FileStoringService.Web.Common.Responses
{
    public class ApiMessageResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public static ApiMessageResponse Ok(string message) =>
            new() { Success = true, Message = message };

        public static ApiMessageResponse Error(string message) =>
            new() { Success = false, Message = message };
    }
}