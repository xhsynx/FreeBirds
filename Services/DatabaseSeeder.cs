using FreeBirds.Data;
using FreeBirds.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FreeBirds.Services
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public DatabaseSeeder(
            AppDbContext context, 
            UserService userService,
            IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
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

            // Get admin user settings from configuration
            var adminSettings = _configuration.GetSection("AdminUser").Get<AdminUserSettings>();
            if (adminSettings == null)
            {
                throw new InvalidOperationException("Admin user settings not found in configuration");
            }

            // Create admin user
            var adminUser = await _userService.CreateUserAsync(
                username: adminSettings.Username,
                password: adminSettings.Password,
                email: adminSettings.Email,
                firstName: adminSettings.FirstName,
                lastName: adminSettings.LastName,
                phoneNumber: adminSettings.PhoneNumber,
                roleId: adminRole.Id
            );

            await _context.SaveChangesAsync();
        }
    }
} 