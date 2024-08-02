using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Student;
using Moq;

namespace EduQuestTests.SectionsTests;
    [TestFixture]
    public class StudentServiceTests
    {
        private Mock<ICourseService> _mockCourseService;
        private Mock<IRepository<int, StudentCourse>> _mockStudentCourseRepo;
        private Mock<IMapper> _mockMapper;
        private StudentService _studentService;

        [SetUp]
        public void Setup()
        {
            _mockCourseService = new Mock<ICourseService>();
            _mockStudentCourseRepo = new Mock<IRepository<int, StudentCourse>>();
            _mockMapper = new Mock<IMapper>();

            _studentService = new StudentService(
                _mockCourseService.Object,
                _mockStudentCourseRepo.Object,
                _mockMapper.Object
            );
        }

        [Test]
        public async Task GetHomeCourses_ReturnsLiveCourses()
        {
            // Arrange
            var courses = new List<CourseDTO>
            {
                new () { Id = 1, CourseStatus = CourseStatusEnum.Live.ToString() },
                new () { Id = 2, CourseStatus = CourseStatusEnum.Draft.ToString() },
                new () { Id = 3, CourseStatus = CourseStatusEnum.Live.ToString() }
            };

            _mockCourseService.Setup(x => x.GetAll()).ReturnsAsync(courses);

            // Act
            var result = await _studentService.GetHomeCourses(1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(c => c.CourseStatus == CourseStatusEnum.Live.ToString()), Is.True);
        }

        [Test]
        public async Task GetRecommendedCourses_ReturnsRecommendedCourses()
        {
            // Arrange
            int userId = 1;
            var enrolledCourses = new List<CourseDTO>
            {
                new CourseDTO { Id = 1, EducatorId = 2, CourseStatus = CourseStatusEnum.Live.ToString() }
            };

            var allCourses = new List<CourseDTO>
            {
                new CourseDTO { Id = 1, EducatorId = 2, CourseStatus = CourseStatusEnum.Live.ToString() },
                new CourseDTO { Id = 2, EducatorId = 3, CourseStatus = CourseStatusEnum.Live.ToString() },
                new CourseDTO { Id = 3, EducatorId = 1, CourseStatus = CourseStatusEnum.Live.ToString() },
                new CourseDTO { Id = 4, EducatorId = 4, CourseStatus = CourseStatusEnum.Draft.ToString() }
            };

            _mockCourseService.Setup(x => x.GetCoursesForStudent(userId)).ReturnsAsync(enrolledCourses);
            _mockCourseService.Setup(x => x.GetAll()).ReturnsAsync(allCourses);

            // Act
            var result = await _studentService.GetRecommendedCourses(userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(2));
        }

        [Test]
        public async Task UserOwnsCourse_ReturnsTrue_WhenUserOwnsCourse()
        {
            // Arrange
            int studentId = 1;
            int courseId = 2;
            var enrolledCourses = new List<CourseDTO>
            {
                new CourseDTO { Id = 1 },
                new CourseDTO { Id = 2 },
                new CourseDTO { Id = 3 }
            };

            _mockCourseService.Setup(x => x.GetCoursesForStudent(studentId)).ReturnsAsync(enrolledCourses);

            // Act
            var result = await _studentService.UserOwnsCourse(studentId, courseId);

            // Assert
            Assert.That(result.StudentId, Is.EqualTo(studentId));
            Assert.That(result.CourseId, Is.EqualTo(courseId));
            Assert.That(result.UserOwnsCourse, Is.True);
        }

        [Test]
        public async Task UserOwnsCourse_ReturnsFalse_WhenUserDoesNotOwnCourse()
        {
            // Arrange
            int studentId = 1;
            int courseId = 4;
            var enrolledCourses = new List<CourseDTO>
            {
                new CourseDTO { Id = 1 },
                new CourseDTO { Id = 2 },
                new CourseDTO { Id = 3 }
            };

            _mockCourseService.Setup(x => x.GetCoursesForStudent(studentId)).ReturnsAsync(enrolledCourses);

            // Act
            var result = await _studentService.UserOwnsCourse(studentId, courseId);

            // Assert
            Assert.That(result.StudentId, Is.EqualTo(studentId));
            Assert.That(result.CourseId, Is.EqualTo(courseId));
            Assert.That(result.UserOwnsCourse, Is.False);
        }
    }