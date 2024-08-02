using System.Security.Cryptography;
using System.Text;
using EduQuest.Entities;
using EduQuest.Features.Auth;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Users;
using Moq;

namespace EduQuestTests.Auth
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<ITokenService> _mockTokenService;
        private AuthService _authService;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTokenService = new Mock<ITokenService>();
            _authService = new AuthService(_mockUserService.Object, _mockTokenService.Object);
        }

        [Test]
        public async Task Login_ReturnsUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = new User()
            {
                Email = email,
                PasswordHashKey = Encoding.UTF8.GetBytes("testkey"),
                Password =
                    new HMACSHA512(Encoding.UTF8.GetBytes("testkey")).ComputeHash(Encoding.UTF8.GetBytes(password))
            };
            _mockUserService.Setup(service => service.GetByEmailAsync(email)).ReturnsAsync(user);

            var loginRequest = new AuthRequestDto { Email = email, Password = password };

            // Act
            var result = await _authService.Login(loginRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        [Test]
        public void Login_ThrowsInvalidCredsException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var user = new User
            {
                Email = email,
                PasswordHashKey = Encoding.UTF8.GetBytes("testkey"),
                Password = new HMACSHA512(Encoding.UTF8.GetBytes("testkey")).ComputeHash(
                    Encoding.UTF8.GetBytes("wrongpassword"))
            };
            _mockUserService.Setup(service => service.GetByEmailAsync(email)).ReturnsAsync(user);

            var loginRequest = new AuthRequestDto { Email = email, Password = password };

            // Act & Assert
            Assert.ThrowsAsync<InvalideCredsException>(() => _authService.Login(loginRequest));
        }

        [Test]
        public async Task Register_ReturnsUser_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";
            var registerRequest = new RegisterRequestDto
            {
                Email = email,
                Password = password,
                FirstName = "Test",
                LastName = "User"
            };

            _mockUserService.Setup(service => service.GetByEmailAsync(email))
                .ThrowsAsync(new UserNotFoundException());

            var createdUser = new User
            {
                Email = email,
                FirstName = "Test",
                LastName = "User",
                ProfilePictureUrl = ""
            };
            _mockUserService.Setup(service => service.AddAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

            // Act
            var result = await _authService.Register(registerRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
            Assert.AreEqual("Test", result.FirstName);
            Assert.AreEqual("User", result.LastName);
        }

        [Test]
        public void Register_ThrowsUserAlreadyExistsException_WhenUserAlreadyExists()
        {
            // Arrange
            var email = "test@example.com";
            var registerRequest = new RegisterRequestDto
            {
                Email = email,
                Password = "password123",
                FirstName = "Test",
                LastName = "User"
            };

            var existingUser = new User { Email = email };
            _mockUserService.Setup(service => service.GetByEmailAsync(email)).ReturnsAsync(existingUser);

            // Act & Assert
            Assert.ThrowsAsync<UserAlreadyExistsException>(() => _authService.Register(registerRequest));
        }
    }
}