using FreeBirds.Data;
using FreeBirds.DTOs;
using FreeBirds.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeBirds.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserReadDTO>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(user => new UserReadDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                })
                .ToListAsync();
        }

        public async Task<UserReadDTO?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserReadDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserReadDTO> CreateUserAsync(UserCreateDTO userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserReadDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}