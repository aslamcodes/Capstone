using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Videos;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.VideosTests;

[TestFixture]
public class VideoRepoTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _videoRepo = new VideoRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private VideoRepo _videoRepo;

    [Test]
    public async Task GetByContentId_ReturnsCorrectVideo()
    {
        // Arrange
        var video = new Video
        {
            Id = 1,
            ContentId = 1,
            DurationHours = 1,
            DurationMinutes = 30,
            DurationSeconds = 0,
            Url = "https://example.com/video1"
        };
        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        // Act
        var result = await _videoRepo.GetByContentId(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.ContentId, Is.EqualTo(1));
        Assert.That(result.Url, Is.EqualTo("https://example.com/video1"));
    }

    [Test]
    public void GetByContentId_ThrowsExceptionForNonExistentContentId()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await _videoRepo.GetByContentId(999));
        Assert.That(exception.Message, Is.EqualTo("Video not found for the content"));
    }

    [Test]
    public async Task Add_AddsVideoToDatabase()
    {
        // Arrange
        var video = new Video
        {
            ContentId = 1,
            DurationHours = 0,
            DurationMinutes = 45,
            DurationSeconds = 30,
            Url = "https://example.com/newvideo"
        };

        // Act
        var result = await _videoRepo.Add(video);

        // Assert
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.Videos.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ReturnsCorrectVideo()
    {
        // Arrange
        var video = new Video
        {
            Id = 1,
            ContentId = 1,
            DurationHours = 2,
            DurationMinutes = 15,
            DurationSeconds = 0,
            Url = "https://example.com/video2"
        };
        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        // Act
        var result = await _videoRepo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Url, Is.EqualTo("https://example.com/video2"));
    }

    [Test]
    public async Task Update_UpdatesVideoInDatabase()
    {
        // Arrange
        var video = new Video
        {
            Id = 1,
            ContentId = 1,
            DurationHours = 1,
            DurationMinutes = 0,
            DurationSeconds = 0,
            Url = "https://example.com/originalvideo"
        };
        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        // Act
        video.Url = "https://example.com/updatedvideo";
        var result = await _videoRepo.Update(video);

        // Assert
        Assert.That(result.Url, Is.EqualTo("https://example.com/updatedvideo"));
        var updatedVideo = await _context.Videos.FindAsync(1);
        Assert.That(updatedVideo.Url, Is.EqualTo("https://example.com/updatedvideo"));
    }

    [Test]
    public async Task Delete_RemovesVideoFromDatabase()
    {
        // Arrange
        var video = new Video
        {
            Id = 1,
            ContentId = 1,
            DurationHours = 0,
            DurationMinutes = 30,
            DurationSeconds = 0,
            Url = "https://example.com/deletevideo"
        };
        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        // Act
        await _videoRepo.Delete(1);

        // Assert
        Assert.That(_context.Videos.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetAll_ReturnsAllVideos()
    {
        // Arrange
        var videos = new[]
        {
            new Video { Id = 1, ContentId = 1, Url = "https://example.com/video1" },
            new Video { Id = 2, ContentId = 2, Url = "https://example.com/video2" },
            new Video { Id = 3, ContentId = 3, Url = "https://example.com/video3" }
        };
        _context.Videos.AddRange(videos);
        await _context.SaveChangesAsync();

        // Act
        var result = await _videoRepo.GetAll();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Select(v => v.Url),
            Is.EquivalentTo(new[]
                { "https://example.com/video1", "https://example.com/video2", "https://example.com/video3" }));
    }
}