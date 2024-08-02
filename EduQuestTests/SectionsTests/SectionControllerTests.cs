using System.Security.Claims;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Contents;
using EduQuest.Features.Contents.Dto;
using EduQuest.Features.Sections;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests.SectionsTests;

public class SectionControllerTests : IDisposable
{
    private Mock<ISectionService> _mockSectionService;
    private Mock<IContentService> _mockContentService;
    private Mock<IControllerValidator> _mockValidator;
    private SectionController _controller;

    public void Dispose()
    {
        _controller.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        _mockSectionService = new Mock<ISectionService>();
        _mockContentService = new Mock<IContentService>();
        _mockValidator = new Mock<IControllerValidator>();
        _controller =
            new SectionController(_mockSectionService.Object, _mockContentService.Object, _mockValidator.Object);
    }

    [Test]
    public async Task GetContentsForSection_ReturnsOkResult_WithContents()
    {
        // Arrange
        var sectionId = 1;
        var contents = new List<ContentDto>
        {
            new ContentDto() { Id = 1, Title = "Content 1", ContentType = ContentTypeEnum.Article.ToString() },
            new ContentDto { Id = 2, Title = "Content 2", ContentType = ContentTypeEnum.Article.ToString() }
        };

        _mockContentService.Setup(s => s.GetContentBySection(sectionId)).ReturnsAsync(contents);

        // Act
        var result = await _controller.GetContentsForSection(sectionId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(contents, okResult.Value);
    }

    [Test]
    public async Task GetSection_ReturnsOkResult_WithSection()
    {
        // Arrange
        var sectionId = 1;
        var section = new SectionDto { Id = 1, Name = "Section 1" };
        _mockSectionService.Setup(s => s.GetById(sectionId)).ReturnsAsync(section);

        // Act
        var result = await _controller.GetSection(sectionId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(section, okResult.Value);
    }

    [Test]
    public async Task GetSection_ReturnsBadRequest_WhenSectionIdIsInvalid()
    {
        // Arrange
        var sectionId = 0;

        // Act
        var result = await _controller.GetSection(sectionId);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task GetSection_ReturnsNotFound_WhenSectionNotFound()
    {
        // Arrange
        var sectionId = 1;
        _mockSectionService.Setup(s => s.GetById(sectionId))
            .ThrowsAsync(new EntityNotFoundException("Section not found"));

        // Act
        var result = await _controller.GetSection(sectionId);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task CreateSection_ReturnsOkResult_WithSection()
    {
        // Arrange
        var request = new SectionDto { Id = 1, Name = "Section 1", CourseId = 1 };
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
        _mockValidator
            .Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), request.CourseId))
            .Returns(Task.CompletedTask);
        _mockSectionService.Setup(s => s.Add(request)).ReturnsAsync(request);

        // Mock the User property in the controller
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
        };

        // Act
        var result = await _controller.CreateSection(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(request, okResult.Value);
    }

    [Test]
    public async Task CreateSection_ReturnsBadRequest_WhenReferenceConstraintExceptionThrown()
    {
        // Arrange
        var request = new SectionDto { Id = 1, Name = "Section 1", CourseId = 1 };
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
        _mockValidator
            .Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), request.CourseId))
            .Returns(Task.CompletedTask);
        _mockSectionService.Setup(s => s.Add(request)).ThrowsAsync(new ReferenceConstraintException());

        // Mock the User property in the controller
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
        };

        // Act
        var result = await _controller.CreateSection(request);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task CreateSection_ReturnsNotFound_WhenEntityNotFoundExceptionThrown()
    {
        // Arrange
        var request = new SectionDto { Id = 1, Name = "Section 1", CourseId = 1 };
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
        _mockValidator
            .Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), request.CourseId))
            .Returns(Task.CompletedTask);
        _mockSectionService.Setup(s => s.Add(request)).ThrowsAsync(new EntityNotFoundException("Course not found"));

        // Mock the User property in the controller
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
        };

        // Act
        var result = await _controller.CreateSection(request);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public async Task CreateSection_ReturnsUnauthorized_WhenUnAuthorisedUserExeceptionThrown()
    {
        // Arrange
        var request = new SectionDto { Id = 1, Name = "Section 1", CourseId = 1 };
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
        _mockValidator
            .Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), request.CourseId))
            .ThrowsAsync(new UnAuthorisedUserExeception());

        // Mock the User property in the controller
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
        };

        // Act
        var result = await _controller.CreateSection(request);

        // Assert
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
    }

    [Test]
    public async Task UpdateSection_ReturnsOkResult_WithUpdatedSection()
    {
        // Arrange
        var section = new SectionDto { Id = 1, Name = "Updated Section", CourseId = 1 };
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
        _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForSection(It.IsAny<IEnumerable<Claim>>(), section.Id))
            .Returns(Task.CompletedTask);
        _mockValidator
            .Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), section.CourseId))
            .Returns(Task.CompletedTask);
        _mockSectionService.Setup(s => s.Update(section)).ReturnsAsync(section);

        // Mock the User property in the controller
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
        };

        // Act
        var result = await _controller.UpdateSection(section);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(section, okResult.Value);
    }

    [Test]
    public async Task DeleteSection_ReturnsOkResult_WithDeletedSection()
    {
        // Arrange
        var sectionId = 1;
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") };
        _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForSection(It.IsAny<IEnumerable<Claim>>(), sectionId))
            .Returns(Task.CompletedTask);
        _mockContentService.Setup(s => s.DeleteBySection(sectionId)).Returns(Task.CompletedTask);
        _mockSectionService.Setup(s => s.DeleteById(sectionId)).ReturnsAsync(new SectionDto { Id = sectionId });

        // Mock the User property in the controller
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
        };

        // Act
        var result = await _controller.DeleteSection(sectionId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(sectionId, (okResult.Value as SectionDto).Id);
    }
}