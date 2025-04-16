using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FreeBirds.Services;
using System.Globalization;

namespace FreeBirds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TestController : ControllerBase
    {
        private readonly LocalizationService _localizationService;

        public TestController(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tests the localization functionality by returning messages in the specified culture
        /// </summary>
        /// <param name="culture">The culture to use for localization (default: tr-TR)</param>
        /// <returns>Localized messages for the specified culture</returns>
        [HttpGet("localization")]
        public IActionResult TestLocalization([FromQuery] string culture = "tr-TR")
        {
            var messages = new
            {
                RequiredField = _localizationService.GetMessage("RequiredField", culture, "Test"),
                StringLength = _localizationService.GetMessage("StringLength", culture, "Test", "50"),
                EmailAddress = _localizationService.GetMessage("EmailAddress", culture),
                FirstName = _localizationService.GetFieldName("FirstName", culture),
                LastName = _localizationService.GetFieldName("LastName", culture)
            };

            return Ok(new
            {
                Culture = culture,
                Messages = messages
            });
        }

        /// <summary>
        /// Checks the health status of the API
        /// </summary>
        /// <returns>Current health status, timestamp, and environment information</returns>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            });
        }
    }
} 