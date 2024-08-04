using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Articles;
using Moq;

namespace EduQuestTests.Articles;

[TestFixture]
public class ArticleServiceTests
{
    private Mock<IArticleRepo> _mockArticleRepo;
    private Mock<IMapper> _mockMapper;
    private ArticleService _articleService;

    [SetUp]
    public void SetUp()
    {
        _mockArticleRepo = new Mock<IArticleRepo>();
        _mockMapper = new Mock<IMapper>();
        _articleService = new ArticleService(_mockArticleRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetByContentId_ShouldReturnMappedArticleDto()
    {
        // Arrange
        int contentId = 1;
        var article = new Article() { Id = 1, ContentId = contentId };
        var articleDto = new ArticleDto { Id = 1, ContentId = contentId };

        _mockArticleRepo.Setup(repo => repo.GetByContentId(contentId))
            .ReturnsAsync(article);

        _mockMapper.Setup(mapper => mapper.Map<ArticleDto>(article))
            .Returns(articleDto);

        // Act
        var result = await _articleService.GetByContentId(contentId);

        // Assert
        Assert.That(result, Is.EqualTo(articleDto));
        _mockArticleRepo.Verify(repo => repo.GetByContentId(contentId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ArticleDto>(article), Times.Once);
    }

    [Test]
    public async Task GetByContentId_ShouldReturnNull_WhenArticleNotFound()
    {
        // Arrange
        int contentId = 1;

        _mockArticleRepo.Setup(repo => repo.GetByContentId(contentId))
            .ReturnsAsync((Article)null);

        _mockMapper.Setup(mapper => mapper.Map<ArticleDto>(null))
            .Returns((ArticleDto)null);

        // Act
        var result = await _articleService.GetByContentId(contentId);

        // Assert
        Assert.That(result, Is.Null);
        _mockArticleRepo.Verify(repo => repo.GetByContentId(contentId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ArticleDto>(null), Times.Once);
    }
}