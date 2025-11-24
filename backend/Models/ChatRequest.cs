namespace api_back.Models
{
    public class ChatRequest
    {
        public string? Message { get; set; }
        public List<ChatMessage> History { get; set; } = new List<ChatMessage>();
    }
}
