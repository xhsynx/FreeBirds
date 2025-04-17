using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeBirds.Models
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public required string Class { get; set; }

        [Required]
        public required string Address { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public Guid ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; } = null!;
    }
} 