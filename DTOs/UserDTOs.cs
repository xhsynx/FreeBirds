namespace FreeBirds.DTOs
{
    public class UserCreateDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public class UserReadDTO
    {
        public int Id { get; set; }
        public string? Name{ get; set; }
        public string? Email { get; set; }
    }
}