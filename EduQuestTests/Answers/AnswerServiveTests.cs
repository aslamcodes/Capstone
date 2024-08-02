using AutoMapper;
using EduQuest.Entities;
using EduQuest.Features.Answers;
using Moq;

namespace EduQuestTests.Answers;

[TestFixture]
public class AnswersServiceTests
{
    private Mock<IAnswerRepo> _mockAnswerRepo;
    private Mock<IMapper> _mockMapper;
    private AnswersService _answersService;

    [SetUp]
    public void SetUp()
    {
        _mockAnswerRepo = new Mock<IAnswerRepo>();
        _mockMapper = new Mock<IMapper>();
        _answersService = new AnswersService(_mockAnswerRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAnswersForQuestion_ShouldReturnMappedAnswerDtos()
    {
        // Arrange
        int questionId = 1;
        var answers = new List<Answer> { new Answer(), new Answer() };
        var answerDtos = new List<AnswerDto> { new AnswerDto(), new AnswerDto() };

        _mockAnswerRepo.Setup(repo => repo.GetAnswersByQuestion(questionId))
            .ReturnsAsync(answers);

        _mockMapper.Setup(mapper => mapper.Map<List<AnswerDto>>(answers))
            .Returns(answerDtos);

        // Act
        var result = await _answersService.GetAnswersForQuestion(questionId);

        // Assert
        Assert.That(result, Is.EqualTo(answerDtos));
        _mockAnswerRepo.Verify(repo => repo.GetAnswersByQuestion(questionId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<List<AnswerDto>>(answers), Times.Once);
    }

    [Test]
    public async Task GetAnswersForQuestion_ShouldReturnEmptyList_WhenNoAnswersFound()
    {
        // Arrange
        int questionId = 1;
        var emptyList = new List<Answer>();
        var emptyDtoList = new List<AnswerDto>();

        _mockAnswerRepo.Setup(repo => repo.GetAnswersByQuestion(questionId))
            .ReturnsAsync(emptyList);

        _mockMapper.Setup(mapper => mapper.Map<List<AnswerDto>>(emptyList))
            .Returns(emptyDtoList);

        // Act
        var result = await _answersService.GetAnswersForQuestion(questionId);

        // Assert
        Assert.That(result, Is.Empty);
        _mockAnswerRepo.Verify(repo => repo.GetAnswersByQuestion(questionId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<List<AnswerDto>>(emptyList), Times.Once);
    }
}