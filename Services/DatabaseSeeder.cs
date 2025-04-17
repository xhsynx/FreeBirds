using FreeBirds.Data;
using FreeBirds.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeBirds.Services
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public DatabaseSeeder(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SeedAdminUserAsync()
        {
            // Check if any admin user exists
            var adminExists = await _context.Users.AnyAsync(u => u.Role == UserRole.Admin);
            
            if (!adminExists)
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Email = "admin@freebirds.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // Default password, should be changed on first login
                    FirstName = "System",
                    LastName = "Administrator",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _context.Users.AddAsync(adminUser);
                await _context.SaveChangesAsync();
            }
        }
    }
} 