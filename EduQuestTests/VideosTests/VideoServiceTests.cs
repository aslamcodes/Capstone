using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Videos;
using Moq;

namespace EduQuestTests.VideosTests;

[TestFixture]
public class VideoServiceTests
{
    private Mock<IVideoRepo> _mockVideoRepo;
    private Mock<IMapper> _mockMapper;
    private VideoService _videoService;

    [SetUp]
    public void SetUp()
    {
        _mockVideoRepo = new Mock<IVideoRepo>();
        _mockMapper = new Mock<IMapper>();
        _videoService = new VideoService(_mockVideoRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetByContentId_ShouldReturnMappedVideoDto()
    {
        // Arrange
        int contentId = 1;
        var video = new Video() { Id = 1, ContentId = contentId };
        var videoDto = new VideoDto { Id = 1, ContentId = contentId };

        _mockVideoRepo.Setup(repo => repo.GetByContentId(contentId))
            .ReturnsAsync(video);

        _mockMapper.Setup(mapper => mapper.Map<VideoDto>(video))
            .Returns(videoDto);

        // Act
        var result = await _videoService.GetByContentId(contentId);

        // Assert
        Assert.That(result, Is.EqualTo(videoDto));
        _mockVideoRepo.Verify(repo => repo.GetByContentId(contentId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<VideoDto>(video), Times.Once);
    }

    [Test]
    public async Task GetByContentId_ShouldReturnNull_WhenVideoNotFound()
    {
        // Arrange
        int contentId = 1;

        _mockVideoRepo.Setup(repo => repo.GetByContentId(contentId))
            .ReturnsAsync((Video)null);

        _mockMapper.Setup(mapper => mapper.Map<VideoDto>(null))
            .Returns((VideoDto)null);

        // Act
        var result = await _videoService.GetByContentId(contentId);

        // Assert
        Assert.That(result, Is.Null);
        _mockVideoRepo.Verify(repo => repo.GetByContentId(contentId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<VideoDto>(null), Times.Once);
    }
}