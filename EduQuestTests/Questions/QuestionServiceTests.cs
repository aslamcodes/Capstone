using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduQuest.Entities;
using EduQuest.Features.Questions;
using EduQuest.Commons;
using AutoMapper;

namespace EduQuestTests.Questions
{
    [TestFixture]
    public class QuestionServiceTests
    {
        private Mock<IQuestionRepo> _mockQuestionRepo;
        private IMapper _mapper;
        private QuestionService _questionService;

        [SetUp]
        public void Setup()
        {
            _mockQuestionRepo = new Mock<IQuestionRepo>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionDto>();
                cfg.CreateMap<QuestionDto, Question>();
            });
            _mapper = config.CreateMapper();

            _questionService = new QuestionService(_mockQuestionRepo.Object, _mapper);
        }

        [Test]
        public async Task GetQuestionsForContent_ShouldReturnQuestionsForGivenContent()
        {
            // Arrange
            int contentId = 1;
            var questions = new List<Question>
            {
                new Question { Id = 1, ContentId = contentId, QuestionText = "Question 1" },
                new Question { Id = 2, ContentId = contentId, QuestionText = "Question 2" }
            };

            _mockQuestionRepo.Setup(repo => repo.GetQuestionsByContent(contentId)).ReturnsAsync(questions);

            // Act
            var result = await _questionService.GetQuestionsForContent(contentId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Question 1", result[0].QuestionText);
            Assert.AreEqual("Question 2", result[1].QuestionText);
        }

        [Test]
        public async Task GetQuestionsForContent_ShouldReturnEmptyListWhenNoQuestions()
        {
            // Arrange
            int contentId = 1;
            var questions = new List<Question>();

            _mockQuestionRepo.Setup(repo => repo.GetQuestionsByContent(contentId)).ReturnsAsync(questions);

            // Act
            var result = await _questionService.GetQuestionsForContent(contentId);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
