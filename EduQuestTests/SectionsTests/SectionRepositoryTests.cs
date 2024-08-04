using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Sections;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.SectionsTests;

[TestFixture]
public class SectionRepositoryTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _sectionRepo = new SectionRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private SectionRepository _sectionRepo;

    [Test]
    public async Task DeleteByCourse_RemovesSectionsForGivenCourseId()
    {
        // Arrange
        var sections = new[]
        {
            new Section { Id = 1, CourseId = 1, Name = "Section 1", Description = "Description 1", OrderId = 1 },
            new Section { Id = 2, CourseId = 1, Name = "Section 2", Description = "Description 2", OrderId = 2 },
            new Section { Id = 3, CourseId = 2, Name = "Section 3", Description = "Description 3", OrderId = 1 }
        };
        await _context.Sections.AddRangeAsync(sections);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sectionRepo.DeleteByCourse(1);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(s => s.Name), Is.EquivalentTo(new[] { "Section 1", "Section 2" }));
        Assert.That(_context.Sections.Count(), Is.EqualTo(1));
        Assert.That(_context.Sections.Single().Name, Is.EqualTo("Section 3"));
    }

    [Test]
    public async Task Add_AddsSectionToDatabase()
    {
        // Arrange
        var section = new Section
        {
            CourseId = 1,
            Name = "New Section",
            Description = "New Description",
            OrderId = 1
        };

        // Act
        var result = await _sectionRepo.Add(section);

        // Assert
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.Sections.Count(), Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("New Section"));
        Assert.That(result.Description, Is.EqualTo("New Description"));
        Assert.That(result.CourseId, Is.EqualTo(1));
        Assert.That(result.OrderId, Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ReturnsCorrectSection()
    {
        // Arrange
        var section = new Section
        {
            Id = 1,
            CourseId = 1,
            Name = "Test Section",
            Description = "Test Description",
            OrderId = 1
        };
        _context.Sections.Add(section);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sectionRepo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Test Section"));
        Assert.That(result.Description, Is.EqualTo("Test Description"));
        Assert.That(result.CourseId, Is.EqualTo(1));
        Assert.That(result.OrderId, Is.EqualTo(1));
    }

    [Test]
    public async Task Update_UpdatesSectionInDatabase()
    {
        // Arrange
        var section = new Section
        {
            Id = 1,
            CourseId = 1,
            Name = "Original Name",
            Description = "Original Description",
            OrderId = 1
        };
        _context.Sections.Add(section);
        await _context.SaveChangesAsync();

        // Act
        section.Name = "Updated Name";
        section.Description = "Updated Description";
        section.OrderId = 2;
        var result = await _sectionRepo.Update(section);

        // Assert
        Assert.That(result.Name, Is.EqualTo("Updated Name"));
        Assert.That(result.Description, Is.EqualTo("Updated Description"));
        Assert.That(result.OrderId, Is.EqualTo(2));
        var updatedSection = await _context.Sections.FindAsync(1);
        Assert.That(updatedSection.Name, Is.EqualTo("Updated Name"));
        Assert.That(updatedSection.Description, Is.EqualTo("Updated Description"));
        Assert.That(updatedSection.OrderId, Is.EqualTo(2));
    }

    [Test]
    public async Task Delete_RemovesSectionFromDatabase()
    {
        // Arrange
        var section = new Section
        {
            Id = 1,
            CourseId = 1,
            Name = "Test Section",
            Description = "Test Description",
            OrderId = 1
        };
        _context.Sections.Add(section);
        await _context.SaveChangesAsync();

        // Act
        await _sectionRepo.Delete(1);

        // Assert
        Assert.That(_context.Sections.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetAll_ReturnsAllSections()
    {
        // Arrange
        var sections = new[]
        {
            new Section { Id = 1, CourseId = 1, Name = "Section 1", Description = "Description 1", OrderId = 1 },
            new Section { Id = 2, CourseId = 1, Name = "Section 2", Description = "Description 2", OrderId = 2 },
            new Section { Id = 3, CourseId = 2, Name = "Section 3", Description = "Description 3", OrderId = 1 }
        };
        _context.Sections.AddRange(sections);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sectionRepo.GetAll();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Select(s => s.Name), Is.EquivalentTo(new[] { "Section 1", "Section 2", "Section 3" }));
        Assert.That(result.Select(s => s.Description),
            Is.EquivalentTo(new[] { "Description 1", "Description 2", "Description 3" }));
        Assert.That(result.Select(s => s.CourseId), Is.EquivalentTo(new[] { 1, 1, 2 }));
        Assert.That(result.Select(s => s.OrderId), Is.EquivalentTo(new[] { 1, 2, 1 }));
    }
}