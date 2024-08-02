using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure;
using EduQuest.Commons;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EduQuestTests.SectionsTests
{
    public class StudentControllerTests
    {
        private Mock<IStudentService> _mockStudentService;
        private Mock<IControllerValidator> _mockValidator;
        private StudentController _controller;

        [SetUp]
        public void Setup()
        {
            _mockStudentService = new Mock<IStudentService>();
            _mockValidator = new Mock<IControllerValidator>();
            _controller = new StudentController(_mockStudentService.Object, _mockValidator.Object);
        }

        [Test]
        public async Task GetRecommendedCourses_ReturnsOkResult_WithRecommendedCourses()
        {
            // Arrange
            var studentId = 1;
            var recommendedCourses = new List<CourseDTO>
            {
                new CourseDTO { Id = 1, Name = "Course 1" },
                new CourseDTO { Id = 2, Name = "Course 2" }
            };

            _mockStudentService.Setup(s => s.GetRecommendedCourses(studentId)).ReturnsAsync(recommendedCourses);

            // Act
            var result = await _controller.GetRecommendedCourses(studentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(recommendedCourses, okResult.Value);
        }

        [Test]
        public async Task UserOwnsCourse_ReturnsOkResult_WithTrue_WhenUserOwnsCourse()
        {
            // Arrange
            var courseId = 1;
            var userId = 1;
            var userClaims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };

            _mockValidator.Setup(v => v.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(userId);
            _mockStudentService.Setup(s => s.UserOwnsCourse(userId, courseId)).ReturnsAsync(new UserOwnsDto()
            {
                UserOwnsCourse = true
            });

            // Mock the User property in the controller
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(userClaims)) }
            };

            // Act
            var result = await _controller.UserOwnsCourse(courseId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(true, (okResult.Value as UserOwnsDto).UserOwnsCourse);
        }

        [Test]
        public async Task UserOwnsCourse_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            // Arrange
            var courseId = 1;
            var userId = 1;
            var userClaims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            
            // _mockValidator.Setup(v => v.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(userId);
            _mockStudentService.Setup(s => s.UserOwnsCourse(userId, courseId)).ThrowsAsync(new UnAuthorisedUserExeception());

            // Mock the User property in the controller
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(userClaims)) }
            };

            // Act
            var result = await _controller.UserOwnsCourse(courseId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result.Result);
        }

        [Test]
        public async Task GetHomeCourses_ReturnsOkResult_WithHomeCourses()
        {
            // Arrange
            var studentId = 1;
            var homeCourses = new List<CourseDTO>
            {
                new CourseDTO { Id = 1, Name = "Course 1" },
                new CourseDTO { Id = 2, Name = "Course 2" }
            };

            _mockStudentService.Setup(s => s.GetHomeCourses(studentId)).ReturnsAsync(homeCourses);

            // Act
            var result = await _controller.GetHomeCourses(studentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(homeCourses, okResult.Value);
        }
    }
}
