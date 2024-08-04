using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Auth;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests.Auth;
 [TestFixture]
    public class AuthControllerTests : IDisposable
    {
        private Mock<IAuthService> _mockAuthService;
        private Mock<IMapper> _mockMapper;
        private Mock<ITokenService> _mockTokenService;
        private AuthController _controller;

        public void Dispose()
        {
            _controller.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockMapper = new Mock<IMapper>();
            _mockTokenService = new Mock<ITokenService>();
            _controller = new AuthController(_mockAuthService.Object, _mockMapper.Object, _mockTokenService.Object);
        }

        [Test]
        public async Task Login_SuccessfulLogin_ReturnsOkResult()
        {
            // Arrange
            var request = new AuthRequestDto 
            { 
                Email = "test@example.com",
                Password = "password123"
            };
            var user = new User 
            { 
                Id = 1,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                IsEducator = false,
                IsAdmin = false,
                Status = UserStatusEnum.ACTIVE
            };
            var response = new AuthResponseDto 
            { 
                Id = "1",
                IsAdmin = false,
                IsEducator = false
            };
            var token = "sample_token";

            _mockAuthService.Setup(x => x.Login(request)).ReturnsAsync(user);
            _mockMapper.Setup(x => x.Map<AuthResponseDto>(user)).Returns(response);
            _mockTokenService.Setup(x => x.GenerateUserToken(user)).Returns(token);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result.Result;
            Assert.That(okResult.Value, Is.InstanceOf<AuthResponseDto>());
            var returnValue = (AuthResponseDto)okResult.Value;
            Assert.That(returnValue.Token, Is.EqualTo(token));
        }

        [Test]
        public async Task Login_UserNotFound_ReturnsUnauthorized()
        {
            // Arrange
            var request = new AuthRequestDto
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };
            _mockAuthService.Setup(x => x.Login(request)).ThrowsAsync(new UserNotFoundException());

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = (UnauthorizedObjectResult)result.Result;
            Assert.That(unauthorizedResult.Value, Is.InstanceOf<ErrorModel>());
            var returnValue = (ErrorModel)unauthorizedResult.Value;
            Assert.That(returnValue.Status, Is.EqualTo(StatusCodes.Status401Unauthorized));
            Assert.That(returnValue.Message, Is.EqualTo("Invalid Credentials"));
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new AuthRequestDto
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };
            _mockAuthService.Setup(x => x.Login(request)).ThrowsAsync(new InvalideCredsException());

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = (UnauthorizedObjectResult)result.Result;
            Assert.That(unauthorizedResult.Value, Is.InstanceOf<ErrorModel>());
            var returnValue = (ErrorModel)unauthorizedResult.Value;
            Assert.That(returnValue.Status, Is.EqualTo(StatusCodes.Status401Unauthorized));
            Assert.That(returnValue.Message, Is.EqualTo("Invalid Credentials"));
        }

        [Test]
        public async Task Register_SuccessfulRegistration_ReturnsOkResult()
        {
            // Arrange
            var request = new RegisterRequestDto 
            { 
                Email = "newuser@example.com",
                Password = "password123",
                FirstName = "Jane",
                LastName = "Doe"
            };
            var user = new User() 
            { 
                Id = 2,
                Email = "newuser@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                IsEducator = false,
                IsAdmin = false,
                Status = UserStatusEnum.ACTIVE
            };
            var response = new AuthResponseDto 
            { 
                Id = "2",
                IsAdmin = false,
                IsEducator = false
            };
            var token = "new_user_token";

            _mockAuthService.Setup(x => x.Register(request)).ReturnsAsync(user);
            _mockMapper.Setup(x => x.Map<AuthResponseDto>(user)).Returns(response);
            _mockTokenService.Setup(x => x.GenerateUserToken(user)).Returns(token);

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result.Result;
            Assert.That(okResult.Value, Is.InstanceOf<AuthResponseDto>());
            var returnValue = (AuthResponseDto)okResult.Value;
            Assert.That(returnValue.Token, Is.EqualTo(token));
        }

        [Test]
        public async Task Register_UserAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequestDto
            {
                Email = "existing@example.com",
                Password = "password123",
                FirstName = "Existing",
                LastName = "User"
            };
            _mockAuthService.Setup(x => x.Register(request)).ThrowsAsync(new UserAlreadyExistsException());

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result.Result;
            Assert.That(badRequestResult.Value, Is.InstanceOf<ErrorModel>());
            var returnValue = (ErrorModel)badRequestResult.Value;
            Assert.That(returnValue.Status, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(returnValue.Message, Is.EqualTo("User Already Exists"));
        }
    }