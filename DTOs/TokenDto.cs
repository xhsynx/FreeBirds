namespace FreeBirds.DTOs
{
    public class TokenDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
} 