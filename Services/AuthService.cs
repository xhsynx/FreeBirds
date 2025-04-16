using System;
using System.Threading.Tasks;
using FreeBirds.Models;
using FreeBirds.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FreeBirds.Data;
using FreeBirds.DTOs;
using Microsoft.Extensions.Configuration;

namespace FreeBirds.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;


        public AuthService(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService ;

        }

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
        {
            if (loginDto is null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var user = await _userService.AuthenticateUser(loginDto.Email, loginDto.Password);
            if (user is null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto is null)
            {
                throw new ArgumentNullException(nameof(registerDto));
            }

            var user = await _userService.CreateUserAsync(registerDto);
            if (user is null)
            {
                throw new InvalidOperationException("Failed to create user");
            }

            var token = _jwtService.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Email, token, and new password are required");
            }

            return await _userService.ResetPasswordAsync(email, token, newPassword);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email is required");
            }

            return await _userService.GeneratePasswordResetTokenAsync(email);
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown"
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<AuthResponse> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token == null)
            {
                throw new InvalidOperationException("Invalid refresh token");
            }

            if (token.User == null)
            {
                throw new InvalidOperationException("User not found for refresh token");
            }

            if (token.Expires < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Refresh token has expired");
            }

            if (token.Revoked != null)
            {
                throw new InvalidOperationException("Refresh token has been revoked");
            }

            // Revoke the used refresh token
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            token.ReasonRevoked = "Replaced by new token";
            await _context.SaveChangesAsync();

            // Generate new token
            return await GenerateToken(token.User);
        }

        private async Task<AuthResponse> GenerateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = _jwtService.GenerateToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id);

            return new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken is null)
            {
                throw new InvalidOperationException("Invalid refresh token");
            }

            if (!refreshToken.IsActive)
            {
                throw new InvalidOperationException("Refresh token is no longer active");
            }

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            refreshToken.ReasonRevoked = "Revoked by user";

            await _context.SaveChangesAsync();
        }

        public async Task RevokeAllTokensAsync(Guid userId)
        {
            var refreshTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.IsActive)
                .ToListAsync();

            foreach (var token in refreshTokens)
            {
                token.Revoked = DateTime.UtcNow;
                token.RevokedByIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                token.ReasonRevoked = "Revoked all tokens";
            }

            await _context.SaveChangesAsync();
        }
    }
} 