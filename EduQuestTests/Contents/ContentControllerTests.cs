using System.Security.Claims;
using EduQuest.Commons;
using EduQuest.Features.Articles;
using EduQuest.Features.Contents;
using EduQuest.Features.Contents.Dto;
using EduQuest.Features.Videos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests;

[TestFixture]
public class ContentControllerTests : IDisposable
{
    private Mock<IContentService> _mockContentService;
    private Mock<IControllerValidator> _mockControllerValidator;
    private Mock<IVideoService> _mockVideoService;
    private Mock<IArticleService> _mockArticleService;
    private ContentController _contentController;

    public void Dispose()
    {
        _contentController.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        _mockContentService = new Mock<IContentService>();
        _mockControllerValidator = new Mock<IControllerValidator>();
        _mockVideoService = new Mock<IVideoService>();
        _mockArticleService = new Mock<IArticleService>();
        _contentController = new ContentController(
            _mockContentService.Object,
            _mockControllerValidator.Object,
            _mockVideoService.Object,
            _mockArticleService.Object);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.NameIdentifier, "1")
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthenticationType"));
        _contentController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Test]
    public async Task GetContent_WhenContentExists_ReturnsOk()
    {
        // Arrange
        var contentId = 1;
        var contentDto = new ContentDto() { Id = contentId, ContentType = ContentTypeEnum.Article.ToString() };
        _mockContentService.Setup(s => s.GetById(contentId)).ReturnsAsync(contentDto);

        // Act
        var result = await _contentController.GetContent(contentId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(contentDto));
    }

    [Test]
    public async Task GetContent_WhenContentDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var contentId = 1;
        _mockContentService.Setup(s => s.GetById(contentId))
            .ThrowsAsync(new EntityNotFoundException("Content not found"));

        // Act
        var result = await _contentController.GetContent(contentId);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        var errorModel = notFoundResult.Value as ErrorModel;
        Assert.AreEqual(StatusCodes.Status404NotFound, errorModel.Status);
        Assert.AreEqual("Content not found", errorModel.Message);
    }

    [Test]
    public async Task UpdateContent_WhenCalled_UpdatesContent()
    {
        // Arrange
        var contentDto = new ContentDto { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Article.ToString() };
        var updatedContentDto = new ContentDto()
            { Id = 1, SectionId = 1, Title = "Updated", ContentType = ContentTypeEnum.Article.ToString() };
        _mockContentService.Setup(s => s.Update(contentDto)).ReturnsAsync(updatedContentDto);

        // Act
        var result = await _contentController.UpdateContent(contentDto);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(updatedContentDto, okResult.Value);
    }

    [Test]
    public async Task DeleteContent_WhenCalled_DeletesContent()
    {
        // Arrange
        var contentId = 1;
        var contentDto = new ContentDto { Id = contentId, ContentType = ContentTypeEnum.Article.ToString() };
        _mockContentService.Setup(s => s.DeleteById(contentId)).ReturnsAsync(contentDto);

        // Act
        var result = await _contentController.DeleteContent(contentId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(contentDto, okResult.Value);
    }

    [Test]
    public async Task CreateContent_WhenCalled_CreatesContent()
    {
        // Arrange
        var contentDto = new ContentDto { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Article.ToString() };
        var createdContentDto = new ContentDto
            { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Article.ToString() };
        _mockContentService.Setup(s => s.Add(contentDto)).ReturnsAsync(createdContentDto);

        // Act
        var result = await _contentController.CreateContent(contentDto);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(createdContentDto, okResult.Value);
    }

    [Test]
    public async Task CreateContent_WhenContentTypeIsArticle_AddsArticle()
    {
        // Arrange
        var contentDto = new ContentDto { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Article.ToString() };
        var createdContentDto = new ContentDto
            { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Article.ToString() };
        _mockContentService.Setup(s => s.Add(contentDto)).ReturnsAsync(createdContentDto);

        // Act
        await _contentController.CreateContent(contentDto);

        // Assert
        _mockArticleService.Verify(a => a.Add(It.Is<ArticleDto>(ad => ad.ContentId == 1)), Times.Once);
    }

    [Test]
    public async Task CreateContent_WhenContentTypeIsVideo_AddsVideo()
    {
        // Arrange
        var contentDto = new ContentDto { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Video.ToString() };
        var createdContentDto = new ContentDto
            { Id = 1, SectionId = 1, ContentType = ContentTypeEnum.Video.ToString() };
        _mockContentService.Setup(s => s.Add(contentDto)).ReturnsAsync(createdContentDto);

        // Act
        await _contentController.CreateContent(contentDto);

        // Assert
        _mockVideoService.Verify(v => v.Add(It.Is<VideoDto>(vd => vd.ContentId == 1)), Times.Once);
    }
}