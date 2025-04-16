using System.ComponentModel.DataAnnotations;

namespace FreeBirds.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
} 