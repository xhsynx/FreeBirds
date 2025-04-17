using System.ComponentModel.DataAnnotations;

namespace FreeBirds.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        public UserRole? Role { get; set; } = UserRole.RegularUser;

        public bool IsActive { get; set; } = true;

        public int FailedLoginAttempts { get; set; }

        public DateTime? LockoutEnd { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiryTime { get; set; }
    }
}