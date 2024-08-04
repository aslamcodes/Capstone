using NUnit.Framework;
using Moq;
using AutoMapper;
using EduQuest.Features.Questions;
using EduQuest.Features.Auth.Exceptions;
using EduQuest.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EduQuest.Entities;

namespace EduQuestTests.Questions
{
    [TestFixture]
    public class QuestionControllerTests
    {
        private Mock<IQuestionService> _mockQuestionService;
        private Mock<IControllerValidator> _mockValidator;
        private IMapper _mapper;
        private QuestionController _controller;

        [SetUp]
        public void Setup()
        {
            _mockQuestionService = new Mock<IQuestionService>();
            _mockValidator = new Mock<IControllerValidator>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionDto>();
                cfg.CreateMap<QuestionDto, Question>();
                cfg.CreateMap<QuestionRequestDto, QuestionDto>();
            });
            _mapper = config.CreateMapper();

            _controller = new QuestionController(_mockQuestionService.Object, _mapper, _mockValidator.Object);
        }

        [Test]
        public async Task GetQuestionsForContent_ShouldReturnOkResultWithQuestions()
        {
            // Arrange
            int contentId = 1;
            var questions = new List<QuestionDto>
            {
                new QuestionDto { Id = 1, ContentId = contentId, QuestionText = "Question 1" },
                new QuestionDto { Id = 2, ContentId = contentId, QuestionText = "Question 2" }
            };

            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId))
                          .Returns(Task.CompletedTask);
            _mockQuestionService.Setup(service => service.GetQuestionsForContent(contentId)).ReturnsAsync(questions);

            // Act
            var result = await _controller.GetQuestionsForContent(contentId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(questions, okResult.Value);
        }

        [Test]
        public async Task GetQuestionsForContent_ShouldReturnUnauthorizedOnUnAuthorisedUserExeception()
        {
            // Arrange
            int contentId = 1;

            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId))
                          .Throws(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.GetQuestionsForContent(contentId);

            // Assert
            var unauthorizedResult = result.Result as ObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [Test]
        public async Task GetQuestionsForContent_ShouldReturnNotFoundOnEntityNotFoundException()
        {
            // Arrange
            int contentId = 1;

            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId))
                          .Throws(new EntityNotFoundException());

            // Act
            var result = await _controller.GetQuestionsForContent(contentId);

            // Assert
            var notFoundResult = result.Result as ObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task GetQuestionsForContent_ShouldReturnStatusCode500OnException()
        {
            // Arrange
            int contentId = 1;

            _mockValidator.Setup(v => v.ValidateUserPrivilegeForContent(It.IsAny<IEnumerable<Claim>>(), contentId))
                          .Throws(new Exception());

            // Act
            var result = await _controller.GetQuestionsForContent(contentId);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task PostQuestion_ShouldReturnOkResultWithQuestion()
        {
            // Arrange
            var questionRequestDto = new QuestionRequestDto { ContentId = 1, PostedById = 1, QuestionText = "Question 1" };
            var questionDto = new QuestionDto { Id = 1, ContentId = 1, PostedById = 1, PostedOn = DateTime.Now, QuestionText = "Question 1" };

            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), questionRequestDto.PostedById))
                          .Returns(Task.CompletedTask);
            _mockQuestionService.Setup(service => service.Add(It.IsAny<QuestionDto>())).ReturnsAsync(questionDto);

            // Act
            var result = await _controller.PostQuestion(questionRequestDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(questionDto, okResult.Value);
        }

        [Test]
        public async Task PostQuestion_ShouldReturnUnauthorizedOnUnAuthorisedUserExeception()
        {
            // Arrange
            var questionRequestDto = new QuestionRequestDto { ContentId = 1, PostedById = 1, QuestionText = "Question 1" };

            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), questionRequestDto.PostedById))
                          .Throws(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.PostQuestion(questionRequestDto);

            // Assert
            var unauthorizedResult = result.Result as ObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [Test]
        public async Task PostQuestion_ShouldReturnNotFoundOnEntityNotFoundException()
        {
            // Arrange
            var questionRequestDto = new QuestionRequestDto { ContentId = 1, PostedById = 1, QuestionText = "Question 1" };

            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), questionRequestDto.PostedById))
                          .Throws(new EntityNotFoundException());

            // Act
            var result = await _controller.PostQuestion(questionRequestDto);

            // Assert
            var notFoundResult = result.Result as ObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task PostQuestion_ShouldReturnStatusCode500OnException()
        {
            // Arrange
            var questionRequestDto = new QuestionRequestDto { ContentId = 1, PostedById = 1, QuestionText = "Question 1" };

            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), questionRequestDto.PostedById))
                          .Throws(new Exception());

            // Act
            var result = await _controller.PostQuestion(questionRequestDto);

            // Assert
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task DeleteQuestion_ShouldReturnOkResultWithQuestion()
        {
            // Arrange
            int id = 1;
            var question = new QuestionDto { Id = id, ContentId = 1, PostedById = 1, QuestionText = "Question 1" };

            _mockQuestionService.Setup(service => service.GetById(id)).ReturnsAsync(question);
            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), question.PostedById))
                          .Returns(Task.CompletedTask);
            _mockQuestionService.Setup(service => service.DeleteById(id)).ReturnsAsync(question);

            // Act
            var result = await _controller.DeleteQuestion(id);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(question));
            });
        }

        [Test]
        public async Task DeleteQuestion_ShouldReturnUnauthorizedOnUnAuthorisedUserExeception()
        {
            // Arrange
            int id = 1;
            var question = new QuestionDto { Id = id, ContentId = 1, PostedById = 1, QuestionText = "Question 1" };

            _mockQuestionService.Setup(service => service.GetById(id)).ReturnsAsync(question);
            _mockValidator.Setup(v => v.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), question.PostedById))
                          .Throws(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.DeleteQuestion(id);

            // Assert
            var unauthorizedResult = result as ObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [Test]
        public async Task DeleteQuestion_ShouldReturnNotFoundOnEntityNotFoundException()
        {
            // Arrange
            int id = 1;

            _mockQuestionService.Setup(service => service.GetById(id)).Throws(new EntityNotFoundException());

            // Act
            var result = await _controller.DeleteQuestion(id);

            // Assert
            var notFoundResult = result as ObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task DeleteQuestion_ShouldReturnStatusCode500OnException()
        {
            // Arrange
            int id = 1;

            _mockQuestionService.Setup(service => service.GetById(id)).Throws(new Exception());

            // Act
            var result = await _controller.DeleteQuestion(id);

            // Assert
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
    }
}
