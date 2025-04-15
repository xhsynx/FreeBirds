using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FreeBirds.Data;
using FreeBirds.Models;
using BCrypt.Net;

namespace FreeBirds.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private const int MAX_FAILED_ATTEMPTS = 5;
        private const int LOCKOUT_DURATION_MINUTES = 15;
        private const int PASSWORD_RESET_TOKEN_EXPIRY_HOURS = 24;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(string username, string email, string password, string? firstName = null, string? lastName = null, string? phoneNumber = null)
        {
            // Check if username is already taken
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> AuthenticateUser(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            // Check if account is locked
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Account is locked. Please try again later.");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                // Increment failed login attempts
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= MAX_FAILED_ATTEMPTS)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(LOCKOUT_DURATION_MINUTES);
                }
                await _context.SaveChangesAsync();
                return null;
            }

            // Successful login
            user.FailedLoginAttempts = 0;
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateRefreshToken(Guid userId, string refreshToken, DateTime expiryTime)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;
            await _context.SaveChangesAsync();
        }

        public async Task RevokeRefreshToken(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => 
                u.RefreshToken == refreshToken && 
                u.RefreshTokenExpiryTime > DateTime.UtcNow);
        }

        public async Task<string> GeneratePasswordResetToken(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Generate random token
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(PASSWORD_RESET_TOKEN_EXPIRY_HOURS);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task ResetPassword(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => 
                u.PasswordResetToken == token && 
                u.PasswordResetTokenExpiryTime > DateTime.UtcNow);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid or expired token");
            }

            // Hash and save new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiryTime = null;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePassword(Guid userId, string currentPassword, string newPassword)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
            {
                throw new InvalidOperationException("Current password is incorrect");
            }

            // Hash and save new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
        }
    }
}