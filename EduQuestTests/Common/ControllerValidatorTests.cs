using System.Security.Claims;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Contents;
using EduQuest.Features.Contents.Dto;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Orders;
using EduQuest.Features.Sections;
using Moq;

namespace EduQuestTests.Common;

[TestFixture]
public class ControllerValidatorTests
{
    private Mock<ICourseService> _mockCourseService;
    private Mock<IOrderService> _mockOrderService;
    private Mock<IContentService> _mockContentService;
    private Mock<ISectionService> _mockSectionService;
    private ControllerValidator _validator;

    [SetUp]
    public void Setup()
    {
        _mockCourseService = new Mock<ICourseService>();
        _mockOrderService = new Mock<IOrderService>();
        _mockContentService = new Mock<IContentService>();
        _mockSectionService = new Mock<ISectionService>();
        _validator = new ControllerValidator(
            _mockCourseService.Object,
            _mockOrderService.Object,
            _mockContentService.Object,
            _mockSectionService.Object
        );
    }

    private IEnumerable<Claim> CreateTestClaims(int userId)
    {
        return new List<Claim>
        {
            new Claim("uid", userId.ToString())
        };
    }

    [Test]
    public async Task ValidateEducatorPrivilegeForCourse_ShouldNotThrow_WhenEducatorOwnsTheCourse()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var course = new CourseDTO() { Id = 1, EducatorId = 1 };
        _mockCourseService.Setup(s => s.GetById(1)).ReturnsAsync(course);

        // Act & Assert
        Assert.DoesNotThrowAsync(() => _validator.ValidateEducatorPrivilegeForCourse(claims, 1));
    }

    [Test]
    public void ValidateEducatorPrivilegeForCourse_ShouldThrow_WhenEducatorDoesNotOwnTheCourse()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var course = new CourseDTO { Id = 1, EducatorId = 2 };
        _mockCourseService.Setup(s => s.GetById(1)).ReturnsAsync(course);

        // Act & Assert
        Assert.ThrowsAsync<UnAuthorisedUserExeception>(() => _validator.ValidateEducatorPrivilegeForCourse(claims, 1));
    }

    [Test]
    public async Task ValidateEducatorPrivilegeForContent_ShouldNotThrow_WhenEducatorOwnsTheContent()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var content = new ContentDto() { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Article.ToString() };
        var section = new SectionDto { Id = 1, CourseId = 1 };
        var course = new CourseDTO { Id = 1, EducatorId = 1 };
        _mockContentService.Setup(s => s.GetById(1)).ReturnsAsync(content);
        _mockSectionService.Setup(s => s.GetById(1)).ReturnsAsync(section);
        _mockCourseService.Setup(s => s.GetById(1)).ReturnsAsync(course);

        // Act & Assert
        Assert.DoesNotThrowAsync(() => _validator.ValidateEducatorPrivilegeForContent(claims, 1));
    }

    [Test]
    public void ValidateEducatorPrivilegeForContent_ShouldThrow_WhenEducatorDoesNotOwnTheContent()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var content = new ContentDto() { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Article.ToString() };
        var section = new SectionDto { Id = 1, CourseId = 1 };
        var course = new CourseDTO { Id = 1, EducatorId = 2 };
        _mockContentService.Setup(s => s.GetById(1)).ReturnsAsync(content);
        _mockSectionService.Setup(s => s.GetById(1)).ReturnsAsync(section);
        _mockCourseService.Setup(s => s.GetById(1)).ReturnsAsync(course);

        // Act & Assert
        Assert.ThrowsAsync<UnAuthorisedUserExeception>(() => _validator.ValidateEducatorPrivilegeForContent(claims, 1));
    }

    [Test]
    public void GetUserIdFromClaims_ShouldReturnUserId_WhenClaimExists()
    {
        // Arrange
        var claims = CreateTestClaims(1);

        // Act
        var result = _validator.GetUserIdFromClaims(claims);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void GetUserIdFromClaims_ShouldThrow_WhenClaimDoesNotExist()
    {
        // Arrange
        var claims = new List<Claim>();

        // Act & Assert
        Assert.Throws<UnAuthorisedUserExeception>(() => _validator.GetUserIdFromClaims(claims));
    }

    [Test]
    public void ValidateEducatorPrevilege_ShouldNotThrow_WhenUserIdMatchesEducatorId()
    {
        // Arrange
        var claims = CreateTestClaims(1);

        // Act & Assert
        Assert.DoesNotThrowAsync(() => _validator.ValidateEducatorPrevilege(claims, 1));
    }

    [Test]
    public void ValidateEducatorPrevilege_ShouldThrow_WhenUserIdDoesNotMatchEducatorId()
    {
        // Arrange
        var claims = CreateTestClaims(1);

        // Act & Assert
        Assert.ThrowsAsync<UnAuthorisedUserExeception>(() => _validator.ValidateEducatorPrevilege(claims, 2));
    }

    [Test]
    public async Task ValidateUserPrivilageForOrder_ShouldNotThrow_WhenUserOwnsTheOrder()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var order = new OrderDto() { Id = 1, UserId = 1 };
        _mockOrderService.Setup(s => s.GetOrderById(1)).ReturnsAsync(order);

        // Act & Assert
        Assert.DoesNotThrowAsync(() => _validator.ValidateUserPrivilageForOrder(claims, 1));
    }

    [Test]
    public void ValidateUserPrivilageForOrder_ShouldThrow_WhenUserDoesNotOwnTheOrder()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var order = new OrderDto() { Id = 1, UserId = 2 };
        _mockOrderService.Setup(s => s.GetOrderById(1)).ReturnsAsync(order);

        // Act & Assert
        Assert.ThrowsAsync<UnAuthorisedUserExeception>(() => _validator.ValidateUserPrivilageForOrder(claims, 1));
    }

    [Test]
    public async Task ValidateStudentPrivilegeForCourse_ShouldNotThrow_WhenStudentIsEnrolledInCourse()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var enrolledCourses = new List<CourseDTO> { new CourseDTO { Id = 1 } };
        var course = new CourseDTO { Id = 1 };
        _mockCourseService.Setup(s => s.GetCoursesForStudent(1)).ReturnsAsync(enrolledCourses);
        _mockCourseService.Setup(s => s.GetById(1)).ReturnsAsync(course);

        // Act & Assert
        Assert.DoesNotThrowAsync(() => _validator.ValidateStudentPrivilegeForCourse(claims, 1));
    }

    [Test]
    public void ValidateStudentPrivilegeForCourse_ShouldThrow_WhenStudentIsNotEnrolledInCourse()
    {
        // Arrange
        var claims = CreateTestClaims(1);
        var enrolledCourses = new List<CourseDTO> { new CourseDTO { Id = 2 } };
        var course = new CourseDTO { Id = 1 };
        _mockCourseService.Setup(s => s.GetCoursesForStudent(1)).ReturnsAsync(enrolledCourses);
        _mockCourseService.Setup(s => s.GetById(1)).ReturnsAsync(course);

        // Act & Assert
        Assert.ThrowsAsync<UnAuthorisedUserExeception>(() => _validator.ValidateStudentPrivilegeForCourse(claims, 1));
    }
}