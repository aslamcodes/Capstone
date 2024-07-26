using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses;
using Moq;

namespace EduQuestTests.Student
{
    public class CourseServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<ICourseRepo> _courseRepoMock;
        private Mock<IRepository<int, StudentCourse>> _studentCourseRepoMock;
        private Mock<IRepository<int, User>> _userRepoMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _courseRepoMock = new Mock<ICourseRepo>();
            _studentCourseRepoMock = new Mock<IRepository<int, StudentCourse>>();
            _userRepoMock = new Mock<IRepository<int, User>>();
        }

        [Test]
        public async Task GetCoursesByStudent_ShouldReturnCourses()
        {
            int studentId = 1;

            var allCourses = new List<Course>
            {
                new() { Id = 1, Name = "Course 1", Description = "Description 1" , Students = [new User { Id = 2 },new User { Id = 1 } ]},
                new() { Id = 2, Name = "Course 2", Description = "Description 2", Students = [new User { Id = 1 }] },
                new() { Id = 3, Name = "Course 3", Description = "Description 2", Students = [] },
            };

            var expectedCourses = new List<Course>
            {
                new() { Id = 1, Name = "Course 1", Description = "Description 1"},
                new() { Id = 2, Name = "Course 2", Description = "Description 2" }
            };


            _courseRepoMock.Setup(x => x.GetAll()).ReturnsAsync(allCourses);
            var courseRepo = _courseRepoMock.Object;
            var userRepo = _userRepoMock.Object;
            var studetnCourseRepo = _studentCourseRepoMock.Object;
            var courseService = new CourseService(courseRepo, userRepo, studetnCourseRepo, _mapperMock.Object);

            var result = await courseService.GetCoursesForStudent(studentId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedCourses.Count));

            for (int i = 0; i < expectedCourses.Count; i++)
            {
                Assert.That(result[i].Id, Is.EqualTo(expectedCourses[i].Id));
                Assert.That(result[i].Name, Is.EqualTo(expectedCourses[i].Name));
                Assert.That(result[i].Description, Is.EqualTo(expectedCourses[i].Description));
            }

        }

    }
}
