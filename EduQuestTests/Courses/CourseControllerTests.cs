using System.Security.Claims;
using AutoMapper;
using Azure.Storage.Blobs;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Contents;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Sections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests.Courses;

 [TestFixture]
    public class CourseControllerTests : IDisposable
    {
        private Mock<ICourseService> _mockCourseService;
        private Mock<ISectionService> _mockSectionService;
        private Mock<IContentService> _mockContentService;
        private Mock<IControllerValidator> _mockValidator;
        private Mock<BlobServiceClient> _mockBlobService;
        private Mock<IMapper> _mockMapper;
        private CourseController _controller;

        public void Dispose()
        {
            _controller.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _mockCourseService = new Mock<ICourseService>();
            _mockSectionService = new Mock<ISectionService>();
            _mockContentService = new Mock<IContentService>();
            _mockValidator = new Mock<IControllerValidator>();
            _mockBlobService = new Mock<BlobServiceClient>();
            _mockMapper = new Mock<IMapper>();

            _controller = new CourseController(
                _mockCourseService.Object,
                _mockSectionService.Object,
                _mockContentService.Object,
                _mockValidator.Object,
                _mockBlobService.Object,
                _mockMapper.Object
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser@example.com"),
            }));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task CreateCourse_ReturnsOkResult_WhenCourseIsCreated()
        {
            // Arrange
            var request = new CourseRequestDTO { EducatorId = 1 };
            var courseDto = new CourseDTO() { Id = 1, EducatorId = 1 };
            _mockValidator.Setup(v => v.ValidateEducatorPrevilege(It.IsAny<IEnumerable<Claim>>(), 1)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<CourseDTO>(request)).Returns(courseDto);
            _mockCourseService.Setup(s => s.Add(courseDto)).ReturnsAsync(courseDto);

            // Act
            var result = await _controller.CreateCourse(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(courseDto));
        }

        [Test]
        public async Task GetSectionsForCourse_ReturnsOkResult_WhenSectionsExist()
        {
            // Arrange
            int courseId = 1;
            var sections = new List<SectionDto> { new SectionDto { Id = 1, CourseId = courseId } };
            _mockSectionService.Setup(s => s.GetSectionForCourse(courseId)).ReturnsAsync(sections);

            // Act
            var result = await _controller.GetSectionsForCourse(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(sections));
        }

        [Test]
        public async Task GetCourse_ReturnsOkResult_WhenCourseExists()
        {
            // Arrange
            int courseId = 1;
            var course = new CourseDTO { Id = courseId };
            _mockCourseService.Setup(s => s.GetById(courseId)).ReturnsAsync(course);

            // Act
            var result = await _controller.GetCourse(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(course));
        }

        [Test]
        public async Task UpdateCourse_ReturnsOkResult_WhenCourseIsUpdated()
        {
            // Arrange
            var course = new CourseDTO { Id = 1, EducatorId = 1 };
            _mockValidator.Setup(v => v.ValidateEducatorPrevilege(It.IsAny<IEnumerable<Claim>>(), 1)).Returns(Task.CompletedTask);
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), 1)).Returns(Task.CompletedTask);
            _mockCourseService.Setup(s => s.Update(course)).ReturnsAsync(course);

            // Act
            var result = await _controller.UpdateCourse(course);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(course));
        }

        [Test]
        public async Task DeleteCourse_ReturnsOkResult_WhenCourseIsDeleted()
        {
            // Arrange
            int courseId = 1;
            var deletedCourse = new CourseDTO { Id = courseId };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), courseId)).Returns(Task.CompletedTask);
            _mockSectionService.Setup(s => s.DeleteSectionsForCourse(courseId)).ReturnsAsync(new List<Section>());
            _mockCourseService.Setup(s => s.DeleteById(courseId)).ReturnsAsync(deletedCourse);

            // Act
            var result = await _controller.DeleteCourse(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(deletedCourse));
        }

        [Test]
        public async Task GetCoursesForStudent_ReturnsOkResult_WhenCoursesExist()
        {
            // Arrange
            int studentId = 1;
            var courses = new List<CourseDTO> { new CourseDTO { Id = 1 } };
            _mockValidator.Setup(v => v.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(studentId);
            _mockCourseService.Setup(s => s.GetCoursesForStudent(studentId)).ReturnsAsync(courses);

            // Act
            var result = await _controller.GetCoursesForStudent();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(courses));
        }

        [Test]
        public async Task GetCoursesForEducator_ReturnsOkResult_WhenCoursesExist()
        {
            // Arrange
            int educatorId = 1;
            var courses = new List<CourseDTO> { new CourseDTO { Id = 1, EducatorId = educatorId } };
            _mockCourseService.Setup(s => s.GetCoursesForEducator(educatorId)).ReturnsAsync(courses);

            // Act
            var result = await _controller.GetCoursesForEducator(educatorId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(courses));
        }

        [Test]
        public async Task GetCourseValidity_ReturnsOkResult_WhenCourseIsValid()
        {
            // Arrange
            int courseId = 1;
            var validity = new ValidityResponseDto { IsValid = true };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), courseId)).Returns(Task.CompletedTask);
            _mockCourseService.Setup(s => s.GetCourseValidity(courseId)).ReturnsAsync(validity);

            // Act
            var result = await _controller.GetCourseValidity(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(validity));
        }

        [Test]
        public async Task SetCourseUnderReview_ReturnsOkResult_WhenCourseIsSetUnderReview()
        {
            // Arrange
            int courseId = 1;
            var course = new CourseDTO { Id = courseId };
            var validity = new ValidityResponseDto { IsValid = true };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForCourse(It.IsAny<IEnumerable<Claim>>(), courseId)).Returns(Task.CompletedTask);
            _mockCourseService.Setup(s => s.GetCourseValidity(courseId)).ReturnsAsync(validity);
            _mockCourseService.Setup(s => s.SetCourseUnderReview(courseId)).ReturnsAsync(course);

            // Act
            var result = await _controller.SetCourseUnderReview(courseId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(course));
        }

        // Add more tests for other methods like SetCourseThumbnail, SearchCourse, GetCoursesByStatus, SetCourseLive, SetCourseOutdated
    }