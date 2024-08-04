using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Contents;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests;

[TestFixture]
public class ContentRepositoryTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _repo = new ContentRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private ContentRepository _repo;

    [Test]
    public async Task Add_ShouldAddContentToDatabase()
    {
        // Arrange
        var content = new Content
            { Id = 1, Title = "Test Content", SectionId = 1, ContentType = ContentTypeEnum.Video };

        // Act
        var result = await _repo.Add(content);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(await _context.Contents.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ShouldReturnCorrectContent()
    {
        // Arrange
        var content = new Content
            { Id = 1, Title = "Test Content", SectionId = 1, ContentType = ContentTypeEnum.Video };
        content.Video = new Video { Id = 1, Url = "https://example.com/video" };
        await _context.Contents.AddAsync(content);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Title, Is.EqualTo("Test Content"));
        Assert.That(result.Video, Is.Not.Null);
        Assert.That(result.Video.Url, Is.EqualTo("https://example.com/video"));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllContents()
    {
        // Arrange
        var contents = new List<Content>
        {
            new() { Id = 1, Title = "Content 1", SectionId = 1, ContentType = ContentTypeEnum.Video },
            new() { Id = 2, Title = "Content 2", SectionId = 1, ContentType = ContentTypeEnum.Article },
            new() { Id = 3, Title = "Content 3", SectionId = 2, ContentType = ContentTypeEnum.Video }
        };
        await _context.Contents.AddRangeAsync(contents);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAll();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Update_ShouldUpdateExistingContent()
    {
        // Arrange
        var content = new Content
            { Id = 1, Title = "Original Title", SectionId = 1, ContentType = ContentTypeEnum.Video };
        await _context.Contents.AddAsync(content);
        await _context.SaveChangesAsync();

        content.Title = "Updated Title";

        // Act
        var result = await _repo.Update(content);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("Updated Title"));
        var updatedContent = await _context.Contents.FindAsync(1);
        Assert.That(updatedContent.Title, Is.EqualTo("Updated Title"));
    }

    [Test]
    public async Task Delete_ShouldRemoveContentFromDatabase()
    {
        // Arrange
        var content = new Content
            { Id = 1, Title = "Test Content", SectionId = 1, ContentType = ContentTypeEnum.Video };
        await _context.Contents.AddAsync(content);
        await _context.SaveChangesAsync();
        _context.Entry(content).State = EntityState.Detached;

        // Act
        var result = await _repo.Delete(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(await _context.Contents.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public async Task DeleteContentsBySection_ShouldRemoveAllContentsFromSpecifiedSection()
    {
        // Arrange
        var contents = new List<Content>
        {
            new() { Id = 1, Title = "Content 1", SectionId = 1, ContentType = ContentTypeEnum.Video },
            new() { Id = 2, Title = "Content 2", SectionId = 1, ContentType = ContentTypeEnum.Article },
            new() { Id = 3, Title = "Content 3", SectionId = 2, ContentType = ContentTypeEnum.Video }
        };
        await _context.Contents.AddRangeAsync(contents);
        await _context.SaveChangesAsync();

        // Act
        await _repo.DeleteContentsBySection(1);

        // Assert
        Assert.That(await _context.Contents.CountAsync(), Is.EqualTo(1));
        Assert.That(await _context.Contents.AnyAsync(c => c.SectionId == 1), Is.False);
    }

    [Test]
    public async Task GetContentsBySection_ShouldReturnCorrectContents()
    {
        // Arrange
        var contents = new List<Content>
        {
            new() { Id = 1, Title = "Content 1", SectionId = 1, ContentType = ContentTypeEnum.Video },
            new() { Id = 2, Title = "Content 2", SectionId = 1, ContentType = ContentTypeEnum.Article },
            new() { Id = 3, Title = "Content 3", SectionId = 2, ContentType = ContentTypeEnum.Video }
        };
        contents[0].Video = new Video { Id = 1, Url = "https://example.com/video1" };
        contents[2].Video = new Video { Id = 2, Url = "https://example.com/video2" };
        await _context.Contents.AddRangeAsync(contents);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetContentsBySection(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(c => c.SectionId == 1), Is.True);
        Assert.That(result.First(c => c.ContentType == ContentTypeEnum.Video).Video, Is.Not.Null);
    }

    [Test]
    public void GetByKey_ShouldThrowExceptionWhenContentNotFound()
    {
        // Act & Assert
        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repo.GetByKey(1));
    }
}