using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FreeBirds.Services;
using FreeBirds.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace FreeBirds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Only users with admin role can access
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Retrieves the latest error logs
        /// </summary>
        /// <param name="count">Number of logs to retrieve (default: 10)</param>
        /// <returns>List of error logs</returns>
        [HttpGet("errors")]
        [ProducesResponseType(typeof(List<Log>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetErrorLogs([FromQuery] int count = 10)
        {
            try
            {
                var logs = await _logService.GetErrorLogsAsync(count);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all logs
        /// </summary>
        /// <param name="count">Number of logs to retrieve (default: 10)</param>
        /// <returns>List of logs</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Log>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetAllLogs([FromQuery] int count = 10)
        {
            try
            {
                var logs = await _logService.GetAllLogsAsync(count);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 