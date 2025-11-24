namespace api_back.Models
{
    public class ChatMessage
    {
        public string? Role { get; set; } // "user" ou "assistant"
        public string? Content { get; set; }
    }
}
