using System;
using System.ComponentModel.DataAnnotations;

namespace FreeBirds.Models
{
    public class Student
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        public StudentType StudentType { get; set; }

        public string? Location { get; set; } // For map location

        public string? AvatarUrl { get; set; } // Avatar image URL

        [Required]
        [StringLength(50)]
        public required string MotherName { get; set; }

        [Required]
        [StringLength(50)]
        public required string FatherName { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal RegistrationFee { get; set; }

        [Required]
        public DateTime RegistrationEndDate { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public string? Notes { get; set; } // Notes about the student

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public enum StudentType
    {
        Regular,    // Regular student
        Special,    // Special education student
        Scholarship // Scholarship student
    }
} 