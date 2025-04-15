using FreeBirds.Data;
using FreeBirds.DTOs;
using FreeBirds.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace FreeBirds.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(LoginDto userDto)
        {
            // Hash the password before storing
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                Password = hashedPassword // Store the hashed password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> AuthenticateUser(string username, string password)
        {
            // Find user by username
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            // If user not found or password doesn't match
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }
    }
}