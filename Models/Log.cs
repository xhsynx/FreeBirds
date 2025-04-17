using System.ComponentModel.DataAnnotations;

namespace FreeBirds.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Level { get; set; } = string.Empty; // Error, Warning, Info, Debug

        [Required]
        [StringLength(255)]
        public string Message { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? Exception { get; set; }

        [StringLength(255)]
        public string? Source { get; set; }

        [StringLength(255)]
        public string? Action { get; set; }

        [StringLength(255)]
        public string? UserId { get; set; }

        [StringLength(50)]
        public string? IPAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 