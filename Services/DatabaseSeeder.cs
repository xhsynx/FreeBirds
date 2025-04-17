using FreeBirds.Data;
using FreeBirds.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FreeBirds.Services
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;

        public DatabaseSeeder(AppDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task SeedAdminUserAsync()
        {
            // Check if admin user already exists
            var adminExists = await _userService.GetUserByUsernameAsync("admin");
            if (adminExists != null) return;

            // Get admin role
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
            {
                throw new InvalidOperationException("Admin role not found in database");
            }

            // Create admin user
            var adminUser = await _userService.CreateUserAsync(
                username: "admin",
                password: "Admin123!", // You should change this in production
                email: "admin@freebirds.com",
                firstName: "Admin",
                lastName: "User",
                phoneNumber: "1234567890",
                roleId: adminRole.Id
            );

            await _context.SaveChangesAsync();
        }
    }
} 