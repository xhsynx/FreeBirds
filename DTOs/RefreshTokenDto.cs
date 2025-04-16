using System.ComponentModel.DataAnnotations;

namespace FreeBirds.DTOs
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
} 