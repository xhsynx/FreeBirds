using System.ComponentModel.DataAnnotations;

namespace FreeBirds.DTOs
{
    public class LoginDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
    }
} 