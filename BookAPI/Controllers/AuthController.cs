using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookAPI.DTOs;
using BookAPI.Models;
using BookAPI.Repositories;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDto">User registration details</param>
        /// <returns>Authentication response with JWT token</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                if (await _userRepository.UserExistsAsync(registerDto.Username))
                {
                    return BadRequest("Username already exists");
                }

                // Check if email already exists
                var existingEmail = await _userRepository.GetUserByEmailAsync(registerDto.Email);
                if (existingEmail != null)
                {
                    return BadRequest("Email already exists");
                }

                // Hash the password (in production, use a proper hashing library like BCrypt)
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                // Create new user
                var user = new User
                {
                    Username = registerDto.Username,
                    Password = hashedPassword,
                    Email = registerDto.Email,
                    Mobile = registerDto.Mobile
                };

                await _userRepository.CreateUserAsync(user);

                // Generate JWT token
                var token = GenerateJwtToken(user);

                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception in Register: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        /// <param name="loginDto">User login credentials</param>
        /// <returns>Authentication response with JWT token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                // Find user by username
                var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
                if (user == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    return Unauthorized("Invalid username or password");
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);

                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception in Login: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings.GetSection("Key").Value;
            var issuer = jwtSettings.GetSection("Issuer").Value;
            var audience = jwtSettings.GetSection("Audience").Value;
            var expireMinutes = int.Parse(jwtSettings.GetSection("ExpireMinutes").Value ?? "60");

            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key is not configured");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Request a password reset token
        /// </summary>
        /// <param name="requestDto">Password reset request with email</param>
        /// <returns>Success message or error</returns>
        [HttpPost("password-reset-request")]
        public async Task<IActionResult> RequestPasswordReset(PasswordResetRequestDto requestDto)
        {
            try
            {
                // Find user by email
                var user = await _userRepository.GetUserByEmailAsync(requestDto.Email);
                if (user == null)
                {
                    // Return success even if user doesn't exist (security best practice)
                    return Ok("If the email exists, a password reset token has been generated.");
                }

                // Generate a secure random token
                var resetToken = GenerateSecureToken();
                
                // Set token and expiry (24 hours from now)
                user.PasswordResetToken = resetToken;
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);

                await _userRepository.UpdateUserAsync(user);

                // In production, send this token via email
                // Do not return the reset token in the HTTP response for security reasons
                return Ok(new 
                { 
                    message = "Password reset token generated successfully"
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception in RequestPasswordReset: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Reset password using the reset token
        /// </summary>
        /// <param name="resetDto">Password reset details including token and new password</param>
        /// <returns>Success message or error</returns>
        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword(PasswordResetDto resetDto)
        {
            try
            {
                // Validate the token
                var user = await _userRepository.GetUserByResetTokenAsync(resetDto.Token);
                if (user == null)
                {
                    return BadRequest("Invalid or expired reset token");
                }

                // Verify email matches
                if (user.Email != resetDto.Email)
                {
                    return BadRequest("Invalid reset request");
                }

                // Hash the new password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetDto.NewPassword);

                // Update user password and clear reset token
                user.Password = hashedPassword;
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiry = null;

                await _userRepository.UpdateUserAsync(user);

                return Ok("Password has been reset successfully");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception in ResetPassword: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Generate a secure random token for password reset
        /// </summary>
        /// <returns>Base64 encoded secure random token</returns>
        private string GenerateSecureToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
