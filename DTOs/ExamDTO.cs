namespace FreeBirds.DTOs
{
    public class ExamDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int TotalPoints { get; set; }
        public Guid ClassId { get; set; }
        public ClassDTO Class { get; set; } = null!;
        public Guid TeacherId { get; set; }
        public TeacherDTO Teacher { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class CreateExamDTO
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int TotalPoints { get; set; }
        public Guid ClassId { get; set; }
        public Guid TeacherId { get; set; }
        public string? Description { get; set; }
    }
} 