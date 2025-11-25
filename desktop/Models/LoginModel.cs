namespace DesktopSql.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "user";
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public LoginModel User { get; set; }
    }
}
