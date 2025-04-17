using Microsoft.EntityFrameworkCore;
using FreeBirds.Models;

namespace FreeBirds.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some test data with hashed password
            // The password "testpassword" is hashed using BCrypt
            modelBuilder.Entity<User>().HasData(
                new User { 
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), 
                    Username = "testuser", 
                    Password = BCrypt.Net.BCrypt.HashPassword("testpassword") 
                }
            );
        }
    }
}