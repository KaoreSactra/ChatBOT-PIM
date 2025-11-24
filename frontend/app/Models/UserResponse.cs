namespace app.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
