using System;
using System.Collections.Generic;

namespace FreeBirds.DTOs
{
    public class EtutDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid TeacherId { get; set; }
        public TeacherDTO Teacher { get; set; } = null!;
        public Guid ClassId { get; set; }
        public ClassDTO Class { get; set; } = null!;
        public List<StudentDTO> Students { get; set; } = new();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class CreateEtutDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid TeacherId { get; set; }
        public Guid ClassId { get; set; }
    }
} 