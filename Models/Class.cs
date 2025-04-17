using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeBirds.Models
{
    public class Class
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public required string Name { get; set; } // Örnek: "9-A", "10-B"

        [Required]
        [StringLength(50)]
        public required string Grade { get; set; } // Örnek: "9. Sınıf", "10. Sınıf"

        [Required]
        [StringLength(1)]
        public required string Section { get; set; } // Örnek: "A", "B", "C"

        [Required]
        public int Capacity { get; set; }

        public int CurrentStudentCount { get; set; }

        public Guid? ClassTeacherId { get; set; }
        [ForeignKey("ClassTeacherId")]
        public virtual Teacher? ClassTeacher { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedAt { get; set; }
    }
} 