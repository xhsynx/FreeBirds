using FreeBirds.DTOs;
using FreeBirds.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FreeBirds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterDto registerDto)
        {
            var user = await _userService.CreateUserAsync(
                registerDto.Username,
                registerDto.Password,
                registerDto.Email,
                registerDto.FirstName,
                registerDto.LastName,
                registerDto.PhoneNumber
            );
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
    }
}