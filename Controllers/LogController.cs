using FreeBirds.Models;
using FreeBirds.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreeBirds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _logService.GetAllLogsAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogById(int id)
        {
            var log = await _logService.GetLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("level/{level}")]
        public async Task<IActionResult> GetLogsByLevel(string level)
        {
            var logs = await _logService.GetLogsByLevelAsync(level);
            return Ok(logs);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetLogsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var logs = await _logService.GetLogsByDateRangeAsync(startDate, endDate);
            return Ok(logs);
        }
    }
} 