using System.Security.Claims;
using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Courses;
using EduQuest.Features.Reviews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests.ReviewsTests
{
    [TestFixture]
    public class ReviewsControllerTests : IDisposable
    {
        private Mock<IReviewService> _mockReviewService;
        private Mock<ICourseService> _mockCourseService;
        private Mock<IControllerValidator> _mockValidator;
        private Mock<IMapper> _mockMapper;
        private ReviewsController _controller;

        public void Dispose()
        {
            _controller.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _mockReviewService = new Mock<IReviewService>();
            _mockCourseService = new Mock<ICourseService>();
            _mockValidator = new Mock<IControllerValidator>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ReviewsController(
                _mockReviewService.Object,
                _mockCourseService.Object,
                _mockValidator.Object,
                _mockMapper.Object
            );
        }

        [Test]
        public async Task GetReviewsByCourse_ReturnsOkResult_WithReviews()
        {
            // Arrange
            int courseId = 1;
            var expectedReviews = new List<ReviewDto> { new ReviewDto(), new ReviewDto() };
            _mockReviewService.Setup(s => s.GetReviewsByCourse(courseId)).ReturnsAsync(expectedReviews);

            // Act
            var result = await _controller.GetReviewsByCourse(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedReviews));
        }

        [Test]
        public async Task GetReviewsByCourse_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            int courseId = 1;
            _mockReviewService.Setup(s => s.GetReviewsByCourse(courseId)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetReviewsByCourse(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.That(statusCodeResult?.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task CreateReview_ReturnsOkResult_WithCreatedReview()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1 };
            var mappedReviewDto = new ReviewDto();
            var createdReviewDto = new ReviewDto();
            _mockMapper.Setup(m => m.Map<ReviewDto>(reviewRequestDto)).Returns(mappedReviewDto);
            _mockReviewService.Setup(s => s.Add(mappedReviewDto)).ReturnsAsync(createdReviewDto);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            };

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(createdReviewDto));
        }

        [Test]
        public async Task CreateReview_ReturnsUnauthorized_WhenUnAuthorisedUserExceptionOccurs()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1 };
            _mockValidator.Setup(v =>
                    v.ValidateStudentPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), reviewRequestDto.CourseId))
                .ThrowsAsync(new UnAuthorisedUserExeception());

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            };

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            Assert.That(unauthorizedResult?.Value, Is.InstanceOf<ErrorModel>());
            var errorModel = unauthorizedResult.Value as ErrorModel;
            Assert.That(errorModel?.Status, Is.EqualTo(StatusCodes.Status401Unauthorized));
            Assert.That(errorModel.Message, Is.EqualTo("Unauthorized"));
        }

        [Test]
        public async Task CreateReview_ReturnsNotFound_WhenEntityNotFoundExceptionOccurs()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1 };
            _mockMapper.Setup(m => m.Map<ReviewDto>(reviewRequestDto)).Returns(new ReviewDto());
            _mockReviewService.Setup(s => s.Add(It.IsAny<ReviewDto>())).ThrowsAsync(new EntityNotFoundException("Not found"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            };

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult?.Value, Is.InstanceOf<ErrorModel>());
            var errorModel = notFoundResult.Value as ErrorModel;
            Assert.That(errorModel?.Status, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(errorModel.Message, Is.EqualTo("Not found"));
        }

        [Test]
        public async Task CreateReview_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1 };
            _mockMapper.Setup(m => m.Map<ReviewDto>(reviewRequestDto)).Throws(new Exception());

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            };

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.That(statusCodeResult?.StatusCode, Is.EqualTo(500));
        }
    }
}