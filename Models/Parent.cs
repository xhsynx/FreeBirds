using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FreeBirds.Models
{
    public class Parent
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
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public required string Email { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
} 