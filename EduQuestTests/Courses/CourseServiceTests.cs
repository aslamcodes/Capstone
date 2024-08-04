using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;
using EduQuest.Features.Sections;
using Moq;

namespace EduQuestTests.Courses;

  [TestFixture]
    public class CourseServiceTests
    {
        private Mock<ICourseRepo> _mockCourseRepo;
        private Mock<IRepository<int, User>> _mockUserRepo;
        private Mock<IRepository<int, StudentCourse>> _mockStudentCourseRepo;
        private Mock<ISectionService> _mockSectionService;
        private Mock<IMapper> _mockMapper;
        private CourseService _courseService;

        [SetUp]
        public void SetUp()
        {
            _mockCourseRepo = new Mock<ICourseRepo>();
            _mockUserRepo = new Mock<IRepository<int, User>>();
            _mockStudentCourseRepo = new Mock<IRepository<int, StudentCourse>>();
            _mockSectionService = new Mock<ISectionService>();
            _mockMapper = new Mock<IMapper>();
            _courseService = new CourseService(_mockCourseRepo.Object, _mockUserRepo.Object, _mockStudentCourseRepo.Object, _mockSectionService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Add_WhenCalled_AddsCourseAndReturnsCourseDto()
        {
            // Arrange
            var courseDto = new CourseDTO { Name = "Test Course" };
            var course = new Course { Id = 1, Name = "Test Course", CourseStatus = CourseStatusEnum.Draft };
            _mockMapper.Setup(m => m.Map<Course>(courseDto)).Returns(course);
            _mockCourseRepo.Setup(r => r.Add(course)).ReturnsAsync(course);
            _mockMapper.Setup(m => m.Map<CourseDTO>(course)).Returns(courseDto);

            // Act
            var result = await _courseService.Add(courseDto);

            // Assert
            Assert.AreEqual(courseDto.Name, result.Name);
            _mockCourseRepo.Verify(r => r.Add(course), Times.Once);
        }

        [Test]
        public async Task EnrollStudentIntoCourse_WhenCalled_EnrollsStudentAndReturnsCourseDto()
        {
            // Arrange
            const int studentId = 1;
            const int courseId = 1;
            var course = new Course { Id = courseId };
            var courseDto = new CourseDTO { Id = courseId };
            _mockCourseRepo.Setup(r => r.GetByKey(courseId)).ReturnsAsync(course);
            _mockStudentCourseRepo.Setup(r => r.Add(It.IsAny<StudentCourse>())).ReturnsAsync(new StudentCourse()
            {
                CourseId = courseId,
                StudentId = studentId
            });
            _mockMapper.Setup(m => m.Map<CourseDTO>(course)).Returns(courseDto);

            // Act
            var result = await _courseService.EnrollStudentIntoCourse(studentId, courseId);

            // Assert
            Assert.AreEqual(courseId, result.Id);
            _mockStudentCourseRepo.Verify(r => r.Add(It.IsAny<StudentCourse>()), Times.Once);
        }

        [Test]
        public async Task GetCoursesForEducator_WhenCalled_ReturnsCoursesForEducator()
        {
            // Arrange
            var educatorId = 1;
            var courses = new List<Course>
            {
                new Course { Id = 1, EducatorId = educatorId },
                new Course { Id = 2, EducatorId = educatorId }
            };
            var courseDtos = new List<CourseDTO>
            {
                new CourseDTO { Id = 1 },
                new CourseDTO { Id = 2 }
            };
            _mockCourseRepo.Setup(r => r.GetAll()).ReturnsAsync(courses);
            _mockMapper.Setup(m => m.Map<CourseDTO>(It.IsAny<Course>())).Returns((Course course) => new CourseDTO { Id = course.Id });

            // Act
            var result = await _courseService.GetCoursesForEducator(educatorId);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetCoursesForStudent_WhenCalled_ReturnsCoursesForStudent()
        {
            // Arrange
            var studentId = 1;
            var studentCourses = new List<StudentCourse>
            {
                new StudentCourse { StudentId = studentId, Course = new Course { Id = 1 } },
                new StudentCourse { StudentId = studentId, Course = new Course { Id = 2 } }
            };
            var courseDtos = new List<CourseDTO>
            {
                new CourseDTO { Id = 1 },
                new CourseDTO { Id = 2 }
            };
            _mockStudentCourseRepo.Setup(r => r.GetAll()).ReturnsAsync(studentCourses);
            _mockMapper.Setup(m => m.Map<CourseDTO>(It.IsAny<Course>())).Returns((Course course) => new CourseDTO { Id = course.Id });

            // Act
            var result = await _courseService.GetCoursesForStudent(studentId);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetCourseValidity_WhenCalled_ReturnsValidityResponseDto()
        {
            // Arrange
            var courseId = 1;
            var course = new Course { Id = courseId, Price = 100, Description = new string('a', 201), Name = "Test Course", CourseCategoryId = 1 };
            var sections = new List<Section>
            {
                new Section { Id = 1, CourseId = courseId },
                new Section { Id = 2, CourseId = courseId },
                new Section { Id = 3, CourseId = courseId },
                new Section { Id = 4, CourseId = courseId }
            };
            _mockCourseRepo.Setup(r => r.GetByKey(courseId)).ReturnsAsync(course);
            _mockSectionService.Setup(s => s.GetSectionForCourse(courseId)).ReturnsAsync(sections.ConvertAll(_mockMapper.Object.Map<SectionDto>));

            // Act
            var result = await _courseService.GetCourseValidity(courseId);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task GetCoursesByStatus_WhenCalled_ReturnsCoursesWithStatus()
        {
            // Arrange
            var status = CourseStatusEnum.Live;
            var courses = new List<Course>
            {
                new Course { Id = 1, CourseStatus = status },
                new Course { Id = 2, CourseStatus = status }
            };
            var courseDtos = new List<CourseDTO>
            {
                new CourseDTO { Id = 1 },
                new CourseDTO { Id = 2 }
            };
            _mockCourseRepo.Setup(r => r.GetByStatus(status)).ReturnsAsync(courses);
            _mockMapper.Setup(m => m.Map<CourseDTO>(It.IsAny<Course>())).Returns((Course course) => new CourseDTO { Id = course.Id });

            // Act
            var result = await _courseService.GetCoursesByStatus(status);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task SearchCourse_WhenCalled_ReturnsSearchedCourses()
        {
            // Arrange
            var query = "Test";
            var courses = new List<Course>
            {
                new Course { Id = 1, Name = "Test Course" },
                new Course { Id = 2, Name = "Another Test Course" }
            };
            var courseDtos = new List<CourseDTO>
            {
                new CourseDTO { Id = 1 },
                new CourseDTO { Id = 2 }
            };
            _mockCourseRepo.Setup(r => r.GetBySearch(query)).ReturnsAsync(courses);
            _mockMapper.Setup(m => m.Map<CourseDTO>(It.IsAny<Course>())).Returns((Course course) => new CourseDTO { Id = course.Id });

            // Act
            var result = await _courseService.SearchCourse(query);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task SetCourseLive_WhenCalled_SetsCourseStatusToLiveAndReturnsCourseDto()
        {
            // Arrange
            var courseId = 1;
            var course = new Course { Id = courseId, CourseStatus = CourseStatusEnum.Draft };
            var courseDto = new CourseDTO { Id = courseId, CourseStatus = CourseStatusEnum.Live.ToString() };
            _mockCourseRepo.Setup(r => r.GetByKey(courseId)).ReturnsAsync(course);
            _mockCourseRepo.Setup(r => r.Update(course)).ReturnsAsync(course);
            _mockMapper.Setup(m => m.Map<CourseDTO>(course)).Returns(courseDto);

            // Act
            var result = await _courseService.SetCourseLive(courseId);

            // Assert
            Assert.AreEqual(CourseStatusEnum.Live.ToString(), result.CourseStatus);
        }

        [Test]
        public async Task SetCourseUnderReview_WhenCourseIsLive_ThrowsInvalidCourseStatusException()
        {
            // Arrange
            var courseId = 1;
            var course = new Course { Id = courseId, CourseStatus = CourseStatusEnum.Live };
            _mockCourseRepo.Setup(r => r.GetByKey(courseId)).ReturnsAsync(course);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidCourseStatusException>(async () => await _courseService.SetCourseUnderReview(courseId));
            Assert.AreEqual("Course is already live", ex.Message);
        }

        [Test]
        public async Task SetCourseUnderReview_WhenCalled_SetsCourseStatusToReviewAndReturnsCourseDto()
        {
            // Arrange
            var courseId = 1;
            var course = new Course { Id = courseId, CourseStatus = CourseStatusEnum.Draft };
            var courseDto = new CourseDTO { Id = courseId, CourseStatus = CourseStatusEnum.Review.ToString() };
            _mockCourseRepo.Setup(r => r.GetByKey(courseId)).ReturnsAsync(course);
            _mockCourseRepo.Setup(r => r.Update(course)).ReturnsAsync(course);
            _mockMapper.Setup(m => m.Map<CourseDTO>(course)).Returns(courseDto);

            // Act
            var result = await _courseService.SetCourseUnderReview(courseId);

            // Assert
            Assert.AreEqual(CourseStatusEnum.Review.ToString(), result.CourseStatus);
        }

        [Test]
        public async Task SetCourseOutdated_WhenCalled_SetsCourseStatusToOutdatedAndReturnsCourseDto()
        {
            // Arrange
            var courseId = 1;
            var course = new Course { Id = courseId, CourseStatus = CourseStatusEnum.Draft };
            var courseDto = new CourseDTO { Id = courseId, CourseStatus = CourseStatusEnum.Outdated.ToString() };
            _mockCourseRepo.Setup(r => r.GetByKey(courseId)).ReturnsAsync(course);
            _mockCourseRepo.Setup(r => r.Update(course)).ReturnsAsync(course);
            _mockMapper.Setup(m => m.Map<CourseDTO>(course)).Returns(courseDto);

            // Act
            var result = await _courseService.SetCourseOutdated(courseId);

            // Assert
            Assert.AreEqual(CourseStatusEnum.Outdated.ToString(), result.CourseStatus);
        }
    }