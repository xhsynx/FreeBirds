namespace FreeBirds.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Class { get; set; }
        public required string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Guid ParentId { get; set; }
        public ParentDTO? Parent { get; set; }
    }

    public class CreateStudentDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Class { get; set; }
        public required string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Guid ParentId { get; set; }
    }
} 