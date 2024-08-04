using System.Security.Claims;
using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Answers;
using EduQuest.Features.Auth.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EduQuestTests.Answers;

    [TestFixture]
    public class AnswersControllerTests : IDisposable
    {
        private Mock<IAnswerService> _mockAnswerService;
        private Mock<IMapper> _mockMapper;
        private Mock<IControllerValidator> _mockValidator;
        private AnswersController _controller;

        public void Dispose()
        {
            _controller.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _mockAnswerService = new Mock<IAnswerService>();
            _mockMapper = new Mock<IMapper>();
            _mockValidator = new Mock<IControllerValidator>();
            _controller = new AnswersController(_mockAnswerService.Object, _mockMapper.Object, _mockValidator.Object);
            
            // Setup for controller context
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Test]
        public async Task GetAnswersForQuestion_ReturnsOkResult()
        {
            // Arrange
            int questionId = 1;
            var expectedAnswers = new List<AnswerDto> { new AnswerDto() };
            _mockAnswerService.Setup(x => x.GetAnswersForQuestion(questionId)).ReturnsAsync(expectedAnswers);

            // Act
            var result = await _controller.GetAnswersForQuestion(questionId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result.Result;
            Assert.That(okResult.Value, Is.EqualTo(expectedAnswers));
        }

        [Test]
        public async Task GetAnswersForQuestion_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            int questionId = 1;
            _mockAnswerService.Setup(x => x.GetAnswersForQuestion(questionId)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAnswersForQuestion(questionId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = (StatusCodeResult)result.Result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task PostAnswer_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var answerRequestDto = new AnswerRequestDto { AnsweredById = 1 };
            var answerDto = new AnswerDto();
            var expectedAnswer = new AnswerDto();

            _mockValidator.Setup(x => x.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), 1)).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<AnswerDto>(answerRequestDto)).Returns(answerDto);
            _mockAnswerService.Setup(x => x.Add(answerDto)).ReturnsAsync(expectedAnswer);

            // Act
            var result = await _controller.PostAnswer(answerRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result.Result;
            Assert.That(okResult.Value, Is.EqualTo(expectedAnswer));
        }

        [Test]
        public async Task PostAnswer_UnauthorizedUser_ReturnsUnauthorized()
        {
            // Arrange
            var answerRequestDto = new AnswerRequestDto { AnsweredById = 1 };
            _mockValidator.Setup(x => x.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), 1))
                .ThrowsAsync(new UnAuthorisedUserExeception());

            // Act
            var result = await _controller.PostAnswer(answerRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = (UnauthorizedObjectResult)result.Result;
            Assert.That(unauthorizedResult.Value, Is.InstanceOf<ErrorModel>());
            var errorModel = (ErrorModel)unauthorizedResult.Value;
            Assert.That(errorModel.Status, Is.EqualTo(StatusCodes.Status401Unauthorized));
            Assert.That(errorModel.Message, Is.EqualTo("You are not authorised to perform this action"));
        }

        [Test]
        public async Task PostAnswer_NonExistentQuestion_ReturnsBadRequest()
        {
            // Arrange
            var answerRequestDto = new AnswerRequestDto { AnsweredById = 1 };
            var answerDto = new AnswerDto();

            _mockValidator.Setup(x => x.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), 1)).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<AnswerDto>(answerRequestDto)).Returns(answerDto);
            _mockAnswerService.Setup(x => x.Add(answerDto)).ThrowsAsync(new ReferenceConstraintException());

            // Act
            var result = await _controller.PostAnswer(answerRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result.Result;
            Assert.That(badRequestResult.Value, Is.InstanceOf<ErrorModel>());
            var errorModel = (ErrorModel)badRequestResult.Value;
            Assert.That(errorModel.Status, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(errorModel.Message, Is.EqualTo("The question you are trying to answer does not exist"));
        }

        [Test]
        public async Task PostAnswer_UnexpectedException_ReturnsInternalServerError()
        {
            // Arrange
            var answerRequestDto = new AnswerRequestDto { AnsweredById = 1 };
            var answerDto = new AnswerDto();

            _mockValidator.Setup(x => x.ValidateUserPrivilageForUserId(It.IsAny<IEnumerable<Claim>>(), 1)).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<AnswerDto>(answerRequestDto)).Returns(answerDto);
            _mockAnswerService.Setup(x => x.Add(answerDto)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.PostAnswer(answerRequestDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = (StatusCodeResult)result.Result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
        }
    }
