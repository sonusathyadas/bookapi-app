using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BookAPI.Controllers;
using BookAPI.Repositories;
using BookAPI.Models;
using BookAPI.DTOs;

namespace BookAPI.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockConfig = new Mock<IConfiguration>();

            // Mock JWT configuration
            var mockKeySection = new Mock<IConfigurationSection>();
            mockKeySection.Setup(s => s.Value).Returns("ThisIsAVeryLongSecretKeyForJWT_MinimumLengthRequired");

            var mockIssuerSection = new Mock<IConfigurationSection>();
            mockIssuerSection.Setup(s => s.Value).Returns("BookAPI");

            var mockAudienceSection = new Mock<IConfigurationSection>();
            mockAudienceSection.Setup(s => s.Value).Returns("BookAPIUsers");

            var mockExpireMinutesSection = new Mock<IConfigurationSection>();
            mockExpireMinutesSection.Setup(s => s.Value).Returns("60");

            var mockJwtSection = new Mock<IConfigurationSection>();
            mockJwtSection.Setup(s => s.GetSection("Key")).Returns(mockKeySection.Object);
            mockJwtSection.Setup(s => s.GetSection("Issuer")).Returns(mockIssuerSection.Object);
            mockJwtSection.Setup(s => s.GetSection("Audience")).Returns(mockAudienceSection.Object);
            mockJwtSection.Setup(s => s.GetSection("ExpireMinutes")).Returns(mockExpireMinutesSection.Object);

            _mockConfig.Setup(c => c.GetSection("Jwt")).Returns(mockJwtSection.Object);

            _controller = new AuthController(_mockUserRepo.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task Register_ReturnsOkWithToken_WhenValidData()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "testuser",
                Password = "testpass",
                Email = "test@example.com",
                Mobile = "1234567890"
            };

            _mockUserRepo.Setup(r => r.UserExistsAsync(registerDto.Username)).ReturnsAsync(false);
            _mockUserRepo.Setup(r => r.GetUserByEmailAsync(registerDto.Email)).ReturnsAsync((User?)null);
            _mockUserRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<AuthResponseDto>(ok.Value);
            Assert.NotEmpty(response.Token);
            Assert.Equal("testuser", response.Username);
            Assert.Equal("test@example.com", response.Email);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUsernameExists()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "existinguser",
                Password = "testpass",
                Email = "test@example.com",
                Mobile = "1234567890"
            };

            _mockUserRepo.Setup(r => r.UserExistsAsync(registerDto.Username)).ReturnsAsync(true);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Username already exists", badRequest.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailExists()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Password = "testpass",
                Email = "existing@example.com",
                Mobile = "1234567890"
            };

            var existingUser = new User
            {
                Id = 1,
                Username = "existinguser",
                Email = "existing@example.com",
                Password = "hashedpassword",
                Mobile = "0987654321"
            };

            _mockUserRepo.Setup(r => r.UserExistsAsync(registerDto.Username)).ReturnsAsync(false);
            _mockUserRepo.Setup(r => r.GetUserByEmailAsync(registerDto.Email)).ReturnsAsync(existingUser);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Email already exists", badRequest.Value);
        }

        [Fact]
        public async Task Login_ReturnsOkWithToken_WhenValidCredentials()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "testpass"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("testpass");
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Password = hashedPassword,
                Email = "test@example.com",
                Mobile = "1234567890"
            };

            _mockUserRepo.Setup(r => r.GetUserByUsernameAsync(loginDto.Username)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<AuthResponseDto>(ok.Value);
            Assert.NotEmpty(response.Token);
            Assert.Equal("testuser", response.Username);
            Assert.Equal("test@example.com", response.Email);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "nonexistent",
                Password = "testpass"
            };

            _mockUserRepo.Setup(r => r.GetUserByUsernameAsync(loginDto.Username)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal("Invalid username or password", unauthorized.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIncorrect()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "wrongpass"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("testpass");
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Password = hashedPassword,
                Email = "test@example.com",
                Mobile = "1234567890"
            };

            _mockUserRepo.Setup(r => r.GetUserByUsernameAsync(loginDto.Username)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result.Result);
            Assert.Equal("Invalid username or password", unauthorized.Value);
        }
    }
}
