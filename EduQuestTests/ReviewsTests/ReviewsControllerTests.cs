using NUnit.Framework;
using Moq;
using AutoMapper;
using EduQuest.Features.Reviews;
using EduQuest.Features.Courses;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EduQuest.Entities;

namespace EduQuestTests.ReviewsTests
{
    [TestFixture]
    public class ReviewsControllerTests : IDisposable
    {
        private Mock<IReviewService> _mockReviewService;
        private Mock<ICourseService> _mockCourseService;
        private Mock<IControllerValidator> _mockValidator;
        private IMapper _mapper;
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

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewRequestDto, ReviewDto>();
                cfg.CreateMap<ReviewDto, Review>();
            });
            _mapper = config.CreateMapper();

            _controller = new ReviewsController(_mockReviewService.Object, _mockCourseService.Object,
                _mockValidator.Object, _mapper);
        }

        [Test]
        public async Task GetReviewsByCourse_ShouldReturnOkResultWithReviews()
        {
            // Arrange
            int courseId = 1;
            var reviews = new List<ReviewDto>
            {
                new ReviewDto { Id = 1, CourseId = courseId, ReviewedById = 1 },
                new ReviewDto { Id = 2, CourseId = courseId, ReviewedById = 2 }
            };

            _mockReviewService.Setup(service => service.GetReviewsByCourse(courseId)).ReturnsAsync(reviews);

            // Act
            var result = await _controller.GetReviewsByCourse(courseId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(reviews, okResult.Value);
        }

        [Test]
        public async Task GetReviewsByCourse_ShouldReturnStatusCode500OnException()
        {
            // Arrange
            int courseId = 1;
            _mockReviewService.Setup(service => service.GetReviewsByCourse(courseId)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetReviewsByCourse(courseId);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task CreateReview_ShouldReturnOkResultWithReview()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1, ReviewedById = 1 };
            var reviewDto = new ReviewDto { Id = 1, CourseId = 1, ReviewedById = 1 };

            _mockValidator.Setup(v =>
                    v.ValidateStudentPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), reviewRequestDto.CourseId))
                .Returns(Task.CompletedTask);

            _mockReviewService.Setup(service => service.Add(It.IsAny<ReviewDto>())).ReturnsAsync(reviewDto);

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(reviewDto, okResult.Value);
        }

        [Test]
        public async Task CreateReview_ShouldReturnUnauthorizedOnUnAuthorisedUserExeception()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1, ReviewedById = 1 };

            _mockValidator.Setup(v =>
                    v.ValidateStudentPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), reviewRequestDto.CourseId))
                .Throws(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            var unauthorizedResult = result as ObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
            Assert.AreEqual("Unauthorized", (unauthorizedResult.Value as ErrorModel)?.Message);
        }

        [Test]
        public async Task CreateReview_ShouldReturnNotFoundOnEntityNotFoundException()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1, ReviewedById = 1 };

            _mockValidator.Setup(v =>
                    v.ValidateStudentPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), reviewRequestDto.CourseId))
                .Throws(new EntityNotFoundException("Not Found"));

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            var notFoundResult = result as ObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Not Found", (notFoundResult.Value as ErrorModel)?.Message);
        }

        [Test]
        public async Task CreateReview_ShouldReturnStatusCode500OnException()
        {
            // Arrange
            var reviewRequestDto = new ReviewRequestDto { CourseId = 1, ReviewedById = 1 };

            _mockValidator.Setup(v =>
                    v.ValidateStudentPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), reviewRequestDto.CourseId))
                .Throws(new Exception());

            // Act
            var result = await _controller.CreateReview(reviewRequestDto);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
    }
}