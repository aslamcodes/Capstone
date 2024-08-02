using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.CourseCategories;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.CatergoryTests;

[TestFixture]
public class CategoryRepoTests
{
    private EduQuestContext _context;
    private CategoryRepo _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        _context = new EduQuestContext(options);
        _repo = new CategoryRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Add_ShouldAddCourseCategoryToDatabase()
    {
        // Arrange
        var category = new CourseCategory { Id = 1, Name = "Test Category", Description = "Test Description" };

        // Act
        var result = await _repo.Add(category);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Test Category"));
        Assert.That(result.Description, Is.EqualTo("Test Description"));
        Assert.That(await _context.CourseCategories.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ShouldReturnCorrectCourseCategory()
    {
        // Arrange
        var category = new CourseCategory { Id = 1, Name = "Test Category", Description = "Test Description" };
        await _context.CourseCategories.AddAsync(category);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Test Category"));
        Assert.That(result.Description, Is.EqualTo("Test Description"));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllCourseCategories()
    {
        // Arrange
        var categories = new List<CourseCategory>
        {
            new CourseCategory { Id = 1, Name = "Category 1", Description = "Description 1" },
            new CourseCategory { Id = 2, Name = "Category 2", Description = "Description 2" },
            new CourseCategory { Id = 3, Name = "Category 3", Description = "Description 3" }
        };
        await _context.CourseCategories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAll();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.Select(c => c.Name), Is.EquivalentTo(new[] { "Category 1", "Category 2", "Category 3" }));
    }

    [Test]
    public async Task Update_ShouldUpdateExistingCourseCategory()
    {
        // Arrange
        var category = new CourseCategory { Id = 1, Name = "Original Name", Description = "Original Description" };
        await _context.CourseCategories.AddAsync(category);
        await _context.SaveChangesAsync();
        category.Name = "Updated Name";
        category.Description = "Updated Description";

        // Act
        var result = await _repo.Update(category);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Updated Name"));
        Assert.That(result.Description, Is.EqualTo("Updated Description"));
        var updatedCategory = await _context.CourseCategories.FindAsync(1);
        Assert.That(updatedCategory.Name, Is.EqualTo("Updated Name"));
        Assert.That(updatedCategory.Description, Is.EqualTo("Updated Description"));
    }

    [Test]
    public async Task Delete_ShouldRemoveCourseCategoryFromDatabase()
    {
        // Arrange
        var category = new CourseCategory { Id = 1, Name = "Test Category", Description = "Test Description" };
        await _context.CourseCategories.AddAsync(category);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.Delete(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(await _context.CourseCategories.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public void GetByKey_ShouldThrowExceptionWhenCategoryNotFound()
    {
        // Act & Assert
        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repo.GetByKey(1));
    }

    [Test]
    public async Task Add_ShouldCreateEmptyCoursesCollection()
    {
        // Arrange
        var category = new CourseCategory() { Id = 1, Name = "Test Category", Description = "Test Description" };

        // Act
        var result = await _repo.Add(category);

        // Assert
        Assert.That(result.Courses, Is.Not.Null);
        Assert.That(result.Courses, Is.Empty);
    }
}