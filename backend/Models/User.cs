// models/User.cs
using System.ComponentModel.DataAnnotations;

namespace api_back.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string PasswordHash { get; set; } = default!;
        [Required]
        public string Role { get; set; } = default!;
    }
}