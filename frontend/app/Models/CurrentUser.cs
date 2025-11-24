namespace app.Models
{
    public class CurrentUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        
        public bool IsAdmin => Role == "admin";
    }
}
