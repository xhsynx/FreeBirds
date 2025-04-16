using FreeBirds.Data;
using FreeBirds.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
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

        public async Task LogErrorAsync(Exception exception, string source)
        {
            var log = new Log
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace ?? string.Empty,
                Source = source,
                CreatedAt = DateTime.UtcNow,
                Level = "Error"
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogWarningAsync(string message, string source)
        {
            var log = new Log
            {
                Message = message,
                Source = source,
                CreatedAt = DateTime.UtcNow,
                Level = "Warning"
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogInfoAsync(string message, string source)
        {
            var log = new Log
            {
                Message = message,
                Source = source,
                CreatedAt = DateTime.UtcNow,
                Level = "Info"
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Log>> GetErrorLogsAsync(int count = 10)
        {
            return await _context.Logs
                .Where(l => l.Level == "Error")
                .OrderByDescending(l => l.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Log>> GetAllLogsAsync(int count = 10)
        {
            return await _context.Logs
                .OrderByDescending(l => l.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
} 