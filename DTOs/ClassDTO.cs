namespace FreeBirds.DTOs
{
    public class ClassDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Grade { get; set; }
        public required string Section { get; set; }
        public int Capacity { get; set; }
        public int CurrentStudentCount { get; set; }
        public Guid? ClassTeacherId { get; set; }
        public TeacherDTO? ClassTeacher { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public class CreateClassDTO
    {
        public required string Name { get; set; }
        public required string Grade { get; set; }
        public required string Section { get; set; }
        public int Capacity { get; set; }
        public Guid? ClassTeacherId { get; set; }
    }
} 