namespace MovieStreaming.Domain.Common.Errors
{
    public class Error
    {
        public static readonly Error None = new Error(string.Empty,string.Empty);
        public string? Code {  get; set; } = string.Empty;
        public string? Message { get; set; } = string.Empty;
        public Error(string code,string message)
        {
            Code = code;
            Message = message;
        }
    }
}
