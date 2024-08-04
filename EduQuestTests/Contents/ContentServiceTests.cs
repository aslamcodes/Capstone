using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Contents;
using EduQuest.Features.Contents.Dto;
using Moq;

namespace EduQuestTests.Contents;

[TestFixture]
public class ContentServiceTests
{
    private Mock<IContentRepo> _mockContentRepo;
    private Mock<IMapper> _mockMapper;
    private ContentService _contentService;

    [SetUp]
    public void SetUp()
    {
        _mockContentRepo = new Mock<IContentRepo>();
        _mockMapper = new Mock<IMapper>();
        _contentService = new ContentService(_mockContentRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task DeleteBySection_WhenCalled_DeletesContentsBySection()
    {
        // Arrange
        var sectionId = 1;

        // Act
        await _contentService.DeleteBySection(sectionId);

        // Assert
        _mockContentRepo.Verify(r => r.DeleteContentsBySection(sectionId), Times.Once);
    }

    [Test]
    public async Task GetContentBySection_WhenCalled_ReturnsContentsBySection()
    {
        // Arrange
        var sectionId = 1;
        var contents = new List<Content>
        {
            new Content { Id = 1, OrderId = 2 },
            new Content { Id = 2, OrderId = 1 }
        };
        var contentDtos = new List<ContentDto>
        {
            new ContentDto { Id = 1, OrderIndex = 2, ContentType = ContentTypeEnum.Article.ToString() },
            new ContentDto { Id = 2, OrderIndex = 1, ContentType = ContentTypeEnum.Article.ToString() }
        };
        _mockContentRepo.Setup(r => r.GetContentsBySection(sectionId)).ReturnsAsync(contents);
        _mockMapper.Setup(m => m.Map<ContentDto>(It.IsAny<Content>())).Returns((Content content) =>
            new ContentDto { Id = content.Id, OrderIndex = content.OrderId, ContentType = content.ContentType.ToString() });

        // Act
        var result = await _contentService.GetContentBySection(sectionId);

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(1, result.First().OrderIndex);
        _mockContentRepo.Verify(r => r.GetContentsBySection(sectionId), Times.Once);
    }
}