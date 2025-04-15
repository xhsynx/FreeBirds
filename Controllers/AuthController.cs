using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FreeBirds.Services;
using FreeBirds.DTOs;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace FreeBirds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;

        public AuthController(UserService userService, JwtService jwtService, EmailService emailService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(
                    registerDto.Username,
                    registerDto.Email,
                    registerDto.Password,
                    registerDto.FirstName,
                    registerDto.LastName,
                    registerDto.PhoneNumber
                );

                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();
                var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

                await _userService.UpdateRefreshToken(user.Id, refreshToken, refreshTokenExpiryTime);

                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.AuthenticateUser(loginDto.Username, loginDto.Password);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();
                var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

                await _userService.UpdateRefreshToken(user.Id, refreshToken, refreshTokenExpiryTime);

                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var user = await _userService.GetUserByRefreshToken(refreshTokenDto.RefreshToken);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                var accessToken = _jwtService.GenerateAccessToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();
                var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

                await _userService.UpdateRefreshToken(user.Id, newRefreshToken, refreshTokenExpiryTime);

                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresIn = 3600
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var user = await _userService.GetUserByRefreshToken(refreshTokenDto.RefreshToken);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid refresh token" });
            }

            await _userService.RevokeRefreshToken(user.Id);
            return Ok(new { message = "Token successfully revoked" });
        }

        [HttpGet("validate")]
        [Authorize]
        public IActionResult Validate()
        {
            return Ok(new { message = "Token is valid" });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var token = await _userService.GeneratePasswordResetToken(forgotPasswordDto.Email);
                var user = await _userService.GetUserByEmailAsync(forgotPasswordDto.Email);
                if (user != null)
                {
                    var emailBody = _emailService.GeneratePasswordResetEmailBody(user.Username, token);
                    await _emailService.SendEmailAsync(user.Email, "Password Reset", emailBody);
                }
                return Ok(new { message = "Password reset email sent" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                await _userService.ResetPassword(resetPasswordDto.Token, resetPasswordDto.NewPassword);
                return Ok(new { message = "Password reset successful" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found"));
                await _userService.UpdatePassword(userId, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);
                return Ok(new { message = "Password updated successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 