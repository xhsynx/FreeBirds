using FreeBirds.Data;
using FreeBirds.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeBirds.Services
{
    public class LogService
    {
        private readonly AppDbContext _context;

        public LogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string level, string message, string? exception = null, string? source = null, string? action = null, string? userId = null, string? ipAddress = null)
        {
            var log = new Log
            {
                Level = level,
                Message = message,
                Exception = exception,
                Source = source,
                Action = action,
                UserId = userId,
                IPAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<Log?> GetLogByIdAsync(int id)
        {
            return await _context.Logs.FindAsync(id);
        }

        public async Task<List<Log>> GetAllLogsAsync()
        {
            return await _context.Logs
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogsByLevelAsync(string level)
        {
            return await _context.Logs
                .Where(l => l.Level == level)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Logs
                .Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }
    }
} 