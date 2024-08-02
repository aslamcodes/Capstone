using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.StudentCourses;
using Microsoft.EntityFrameworkCore;
using StudentCourseRepo = EduQuest.Entities.StudentCourseRepo;

namespace EduQuestTests.Courses;

[TestFixture]
public class StudentCourseRepoTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _studentCourseRepo = new EduQuest.Features.StudentCourses.StudentCourseRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private EduQuest.Features.StudentCourses.StudentCourseRepo _studentCourseRepo;

    [Test]
    public async Task GetAll_ReturnsAllStudentCoursesWithCourses()
    {
        // Arrange
        var courses = new[]
        {
            new Course { Id = 1, Name = "Course 1", Description = "Test" },
            new Course { Id = 2, Name = "Course 2" , Description = "Test" }
        };
        _context.Courses.AddRange(courses);

        var studentCourses = new[]
        {
            new StudentCourseRepo { Id = 1, CourseId = 1, StudentId = 1, Course = courses[0] },
            new StudentCourseRepo { Id = 2, CourseId = 2, StudentId = 1, Course = courses[1] },
            new StudentCourseRepo { Id = 3, CourseId = 1, StudentId = 2, Course = courses[0] }
        };
        _context.StudentCourses.AddRange(studentCourses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _studentCourseRepo.GetAll();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.All(sc => sc.Course != null), Is.True);
        Assert.That(result.Select(sc => sc.Course.Name),
            Is.EquivalentTo(new[] { "Course 1", "Course 2", "Course 1" }));
    }

    [Test]
    public async Task Add_AddsStudentCourseToDatabase()
    {
        // Arrange
        var course = new Course { Id = 1, Name = "Test Course", Description = "asdasd"};
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var studentCourse = new StudentCourseRepo { CourseId = 1, StudentId = 1 };

        // Act
        var result = await _studentCourseRepo.Add(studentCourse);

        // Assert
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.StudentCourses.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ReturnsCorrectStudentCourse()
    {
        // Arrange
        var course = new Course { Id = 1, Name = "Test Course", Description = "Test" };
        _context.Courses.Add(course);

        var studentCourse = new StudentCourseRepo { Id = 1, CourseId = 1, StudentId = 1 };
        _context.StudentCourses.Add(studentCourse);
        await _context.SaveChangesAsync();

        // Act
        var result = await _studentCourseRepo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.CourseId, Is.EqualTo(1));
        Assert.That(result.StudentId, Is.EqualTo(1));
    }

    [Test]
    public async Task Update_UpdatesStudentCourseInDatabase()
    {
        // Arrange
        var course = new Course { Id = 1, Name = "Test Course",  Description = "Test" };
        _context.Courses.Add(course);

        var studentCourse = new StudentCourseRepo { Id = 1, CourseId = 1, StudentId = 1 };
        _context.StudentCourses.Add(studentCourse);
        await _context.SaveChangesAsync();

        // Act
        studentCourse.StudentId = 2;
        var result = await _studentCourseRepo.Update(studentCourse);

        // Assert
        Assert.That(result.StudentId, Is.EqualTo(2));
        var updatedStudentCourse = await _context.StudentCourses.FindAsync(1);
        Assert.That(updatedStudentCourse.StudentId, Is.EqualTo(2));
    }

    [Test]
    public async Task Delete_RemovesStudentCourseFromDatabase()
    {
        // Arrange
        var course = new Course { Id = 1, Name = "Test Course", Description = "asdasd"};
        _context.Courses.Add(course);

        var studentCourse = new StudentCourseRepo { Id = 1, CourseId = 1, StudentId = 1, };
        _context.StudentCourses.Add(studentCourse);
        await _context.SaveChangesAsync();

        // Act
        await _studentCourseRepo.Delete(1);

        // Assert
        Assert.That(_context.StudentCourses.Count(), Is.EqualTo(0));
    }
}