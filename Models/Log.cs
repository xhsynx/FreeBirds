using System;

namespace FreeBirds.Models
{
    public class Log
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public string? StackTrace { get; set; }
        public required string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Level { get; set; } // Error, Warning, Info, etc.
    }
} 