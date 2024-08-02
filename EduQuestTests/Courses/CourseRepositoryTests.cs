using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.Courses;

[TestFixture]
public class CourseRepositoryTests
{
    private EduQuestContext _context;
    private CourseRepository _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _context = new EduQuestContext(options);
        _repo = new CourseRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Add_ShouldAddCourseToDatabase()
    {
        // Arrange
        var course = new Course()
        {
            Id = 1, Name = "Test Course", Description = "Test Description", Level = CourseLevelEnum.Beginner
        };

        // Act
        var result = await _repo.Add(course);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(await _context.Courses.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ShouldReturnCorrectCourse()
    {
        // Arrange
        var course = new Course()
        {
            Id = 1, Name = "Test Course", Description = "Test Description", Level = CourseLevelEnum.Beginner
        };
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Test Course"));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllCoursesWithStudents()
    {
        // Arrange
        var courses = new List<Course>
        {
            new EduQuest.Entities.Course()
            {
                Id = 1, Name = "Course 1", Students = new List<User> { new User { Id = 1, FirstName = "Student 1" } }
            },
            new Course
            {
                Id = 2, Name = "Course 2", Students = new List<User> { new User { Id = 2, FirstName = "Student 2" } }
            },
            new Course { Id = 3, Name = "Course 3", Students = new List<User>() }
        };
        await _context.Courses.AddRangeAsync(courses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAll();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.All(c => c.Students != null), Is.True);
        Assert.That(result.Count(c => c.Students.Any()), Is.EqualTo(2));
    }

    [Test]
    public async Task Update_ShouldUpdateExistingCourse()
    {
        // Arrange
        var course = new Course
        {
            Id = 1,
            Name = "Original Name",
            Description = "Original Description",
            Level = CourseLevelEnum.Beginner
        };
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        course.Name = "Updated Name";
        course.Level = CourseLevelEnum.Intermediate;

        // Act
        var result = await _repo.Update(course);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Updated Name"));
        Assert.That(result.Level, Is.EqualTo(CourseLevelEnum.Intermediate));
    }

    [Test]
    public async Task Delete_ShouldRemoveCourseFromDatabase()
    {
        // Arrange
        var course = new Course
        {
            Id = 1, Name = "Test Course", Description = "Test Description", Level = CourseLevelEnum.Beginner
        };
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.Delete(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(await _context.Courses.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetBySearch_ShouldReturnRelevantCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = 1, Name = "Python Programming", Description = "Learn Python" },
            new Course { Id = 2, Name = "Java Basics", Description = "Introduction to Java" },
            new Course { Id = 3, Name = "Advanced Python", Description = "Advanced Python concepts" },
            new Course { Id = 4, Name = "C# Fundamentals", Description = "Learn C# programming" }
        };
        await _context.Courses.AddRangeAsync(courses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetBySearch("python");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Python Programming"));
        Assert.That(result[1].Name, Is.EqualTo("Advanced Python"));
    }

    [Test]
    public async Task GetByStatus_ShouldReturnCoursesWithSpecificStatus()
    {
        // Arrange
        var educator = new User { Id = 1, FirstName = "Educator 1" };
        var courses = new List<Course>
        {
            new Course
            {
                Id = 1, Name = "Course 1", CourseStatus = CourseStatusEnum.Live, Educator = educator
            },
            new Course { Id = 2, Name = "Course 2", CourseStatus = CourseStatusEnum.Draft, Educator = educator },
            new Course { Id = 3, Name = "Course 3", CourseStatus = CourseStatusEnum.Live, Educator = educator }
        };
        await _context.Courses.AddRangeAsync(courses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByStatus(CourseStatusEnum.Live);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(c => c.CourseStatus == CourseStatusEnum.Live), Is.True);
        Assert.That(result.All(c => c.Educator != null), Is.True);
    }
}