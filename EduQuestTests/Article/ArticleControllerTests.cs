using System.Security.Claims;
using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Articles;
using EduQuest.Features.Auth;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Features.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests.Articles;
  [TestFixture]
    public class ArticleControllerTests
    {
        private Mock<IArticleService> _mockArticleService;
        private Mock<IControllerValidator> _mockValidator;
        private ArticleController _controller;

        [SetUp]
        public void Setup()
        {
            _mockArticleService = new Mock<IArticleService>();
            _mockValidator = new Mock<IControllerValidator>();
            _controller = new ArticleController(_mockArticleService.Object, _mockValidator.Object);

            // Setup controller context
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser@example.com"),
            }));
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Test]
        public async Task GetArticleByContentId_ReturnsOkResult_WhenArticleExists()
        {
            // Arrange
            int contentId = 1;
            var articleDto = new ArticleDto { Id = 1, ContentId = contentId };
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId)).Returns(Task.CompletedTask);
            _mockArticleService.Setup(s => s.GetByContentId(contentId)).ReturnsAsync(articleDto);

            // Act
            var result = await _controller.GetArticleByContentId(contentId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(articleDto));
        }

        [Test]
        public async Task GetArticleByContentId_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            // Arrange
            int contentId = 1;
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId))
                .ThrowsAsync(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.GetArticleByContentId(contentId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
        }

        [Test]
        public async Task GetArticleByContentId_ReturnsNotFound_WhenArticleDoesNotExist()
        {
            // Arrange
            int contentId = 1;
            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId)).Returns(Task.CompletedTask);
            _mockArticleService.Setup(s => s.GetByContentId(contentId)).ThrowsAsync(new EntityNotFoundException("Article not found"));

            // Act
            var result = await _controller.GetArticleByContentId(contentId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task CreateArticle_ReturnsOkResult_WhenArticleIsCreated()
        {
            // Arrange
            var articleDto = new ArticleDto { Id = 1, ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), articleDto.ContentId)).Returns(Task.CompletedTask);
            _mockArticleService.Setup(s => s.Add(articleDto)).ReturnsAsync(articleDto);

            // Act
            var result = await _controller.CreateArticle(articleDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(articleDto));
        }

        [Test]
        public async Task CreateArticle_ReturnsUnauthorized_WhenEducatorIsNotAuthorized()
        {
            // Arrange
            var articleDto = new ArticleDto { Id = 1, ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), articleDto.ContentId))
                .ThrowsAsync(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.CreateArticle(articleDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
        }

        [Test]
        public async Task UpdateArticle_ReturnsOkResult_WhenArticleIsUpdated()
        {
            // Arrange
            var articleDto = new ArticleDto { Id = 1, ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), articleDto.ContentId)).Returns(Task.CompletedTask);
            _mockArticleService.Setup(s => s.Update(articleDto)).ReturnsAsync(articleDto);

            // Act
            var result = await _controller.UpdateArticle(articleDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(articleDto));
        }

        [Test]
        public async Task UpdateArticle_ReturnsUnauthorized_WhenEducatorIsNotAuthorized()
        {
            // Arrange
            var articleDto = new ArticleDto { Id = 1, ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), articleDto.ContentId))
                .ThrowsAsync(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.UpdateArticle(articleDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
        }

        [Test]
        public async Task UpdateArticle_ReturnsNotFound_WhenArticleDoesNotExist()
        {
            // Arrange
            var articleDto = new ArticleDto() { Id = 1, ContentId = 1 };
            _mockValidator.Setup(v => v.ValidateEducatorPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), articleDto.ContentId)).Returns(Task.CompletedTask);
            _mockArticleService.Setup(s => s.Update(articleDto)).ThrowsAsync(new EntityNotFoundException("Article not found"));

            // Act
            var result = await _controller.UpdateArticle(articleDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }