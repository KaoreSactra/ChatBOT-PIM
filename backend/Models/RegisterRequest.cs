using System.ComponentModel.DataAnnotations;

namespace api_back.Models
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public string Role { get; set; } = default!;
    }
}