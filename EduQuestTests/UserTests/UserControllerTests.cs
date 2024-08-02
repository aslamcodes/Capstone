using System.Security.Claims;
using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using EduQuest.Commons;
using EduQuest.Features.Questions;
using EduQuest.Features.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EduQuestTests.Controllers;

[TestFixture]
public class UserControllerTests
{
    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        _mockMapper = new Mock<IMapper>();
        _mockValidator = new Mock<IControllerValidator>();
        _mockBlobServiceClient = new Mock<BlobServiceClient>();
        _mockLogger = new Mock<ILogger<UserController>>();

        _controller = new UserController(
            _mockUserService.Object,
            _mockMapper.Object,
            _mockValidator.Object,
            _mockBlobServiceClient.Object,
            _mockLogger.Object
        );

        // Setup controller context
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    private Mock<IUserService> _mockUserService;
    private Mock<IMapper> _mockMapper;
    private Mock<IControllerValidator> _mockValidator;
    private Mock<BlobServiceClient> _mockBlobServiceClient;
    private Mock<ILogger<UserController>> _mockLogger;
    private UserController _controller;

    [Test]
    public async Task GetUserProfile_ReturnsOkResult_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var userProfile = new UserProfileDto { Id = userId, FirstName = "John" };
        _mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(userProfile);

        // Act
        var result = await _controller.GetUserProfile(userId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(userProfile));
    }

    [Test]
    public async Task UpdateUserProfile_ReturnsOkResult_WhenUpdateSucceeds()
    {
        // Arrange
        var updateDto = new UserProfileUpdateDto { Id = 1, FirstName = "John" };
        var updatedProfile = new UserProfileDto { Id = 1, FirstName = "John" };
        _mockUserService.Setup(s => s.UpdateProfileEntries(updateDto)).ReturnsAsync(updatedProfile);

        // Act
        var result = await _controller.UpdateUserProfile(updateDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(updatedProfile));
    }

    [Test]
    public async Task BecomeEducator_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        var userId = 1;
        var updatedUser = new UserProfileDto { Id = userId, IsEducator = true };
        _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), userId))
            .Returns(Task.CompletedTask);
        _mockUserService.Setup(s => s.MakeEducator(userId)).ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.BecomeEducator(userId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(updatedUser));
    }

    [Test]
    public async Task UploadUserProfile_ReturnsOkResult_WhenUploadSucceeds()
    {
        // Arrange
        var userId = 1;
        var file = new Mock<IFormFile>();
        var memoryStream = new MemoryStream();
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockBlobContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();
        mockBlobClient.Setup(b => b.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true ,null));
        mockBlobClient.Setup(b => b.Uri).Returns(new Uri("http://example.com/profile.jpg"));

        _mockBlobServiceClient.Setup(b => b.GetBlobContainerClient(It.IsAny<string>()))
            .Returns(mockBlobContainerClient.Object);
        mockBlobContainerClient.Setup(c => c.GetBlobClient(It.IsAny<string>())).Returns(mockBlobClient.Object);

        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId.ToString()) };
        _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var userProfile = new UserProfileDto { Id = userId, ProfilePictureUrl = "http://example.com/profile.jpg" };
        _mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(userProfile);
        _mockUserService.Setup(s => s.UpdateProfile(It.IsAny<UserProfileDto>())).ReturnsAsync(userProfile);

        // Act
        var result = await _controller.UploadUserProfile(file.Object);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(userProfile));
    }

    [Test]
    public async Task GetEducatorProfile_ReturnsOkResult_WhenEducatorExists()
    {
        // Arrange
        var educatorId = 1;
        var userProfile = new UserProfileDto { Id = educatorId, FirstName = "John", IsEducator = true };
        var educatorProfile = new EducatorProfileDto { FirstName = "John" };
        _mockUserService.Setup(s => s.GetById(educatorId)).ReturnsAsync(userProfile);
        _mockMapper.Setup(m => m.Map<EducatorProfileDto>(userProfile)).Returns(educatorProfile);

        // Act
        var result = await _controller.GetEducatorProfile(educatorId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(educatorProfile));
    }
}