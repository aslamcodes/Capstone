using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Questions;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.Questions;

[TestFixture]
public class QuestionRepoTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _questionRepo = new QuestionRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private QuestionRepo _questionRepo;

    [Test]
    public async Task GetQuestionsByContent_ReturnsCorrectQuestions()
    {
        // Arrange
        var user1 = new User { Id = 1, FirstName = "User 1", LastName = "Test", Email = "tes1t@test.com", Password = [], PasswordHashKey = [], ProfilePictureUrl = "1231"};
        var user2 = new User { Id = 2, FirstName = "User 2", LastName = "Test", Email = "tes2t@test.com", Password = [], PasswordHashKey = [], ProfilePictureUrl = "1231"};
        _context.Users.AddRange(user1, user2);

        var questions = new List<Question>
        {
            new() { Id = 1, ContentId = 1, QuestionText = "Question 1", PostedBy = user1, },
            new() { Id = 2, ContentId = 1, QuestionText = "Question 2", PostedBy = user1, },
            new() { Id = 3, ContentId = 2, QuestionText = "Question 3", PostedBy = user2, }
        };
        _context.Questions.AddRange(questions);
        await _context.SaveChangesAsync();

        // Act
        var result = await _questionRepo.GetQuestionsByContent(1);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(q => q.QuestionText), Is.EquivalentTo(new[] { "Question 1", "Question 2" }));
        Assert.That(result.All(q => q.PostedBy.FirstName == "User 1"), Is.True);
    }

    [Test]
    public async Task Add_AddsQuestionToDatabase()
    {
        // Arrange
        var question = new Question { QuestionText = "New Question", ContentId = 1 };

        // Act
        var result = await _questionRepo.Add(question);

        // Assert
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.Questions.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ReturnsCorrectQuestion()
    {
        // Arrange
        var question = new Question { Id = 1, QuestionText = "Test Question", ContentId = 1 };
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        // Act
        var result = await _questionRepo.GetByKey(1);

        // Assert
        Assert.That(result.QuestionText, Is.EqualTo("Test Question"));
    }

    [Test]
    public async Task GetByKey_ThrowsExceptionForNonExistentKey()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await _questionRepo.GetByKey(999));
        Assert.That(exception.Message, Is.EqualTo("Question with key 999 not found!!!"));
    }

    [Test]
    public async Task Update_UpdatesQuestionInDatabase()
    {
        // Arrange
        var question = new Question { Id = 1, QuestionText = "Original QuestionText", ContentId = 1 };
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        // Act
        question.QuestionText = "Updated QuestionText";
        var result = await _questionRepo.Update(question);

        // Assert
        Assert.That(result.QuestionText, Is.EqualTo("Updated QuestionText"));
        var updatedQuestion = await _context.Questions.FindAsync(1);
        Assert.That(updatedQuestion.QuestionText, Is.EqualTo("Updated QuestionText"));
    }

    [Test]
    public async Task Delete_RemovesQuestionFromDatabase()
    {
        // Arrange
        var question = new Question { Id = 1, QuestionText = "Test Question", ContentId = 1 };
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        // Act
        await _questionRepo.Delete(1);

        // Assert
        Assert.That(_context.Questions.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetAll_ReturnsAllQuestions()
    {
        // Arrange
        var questions = new List<Question>
        {
            new() { Id = 1, QuestionText = "Question 1", ContentId = 1 },
            new() { Id = 2, QuestionText = "Question 2", ContentId = 2 },
            new() { Id = 3, QuestionText = "Question 3", ContentId = 3 }
        };
        _context.Questions.AddRange(questions);
        await _context.SaveChangesAsync();

        // Act
        var result = await _questionRepo.GetAll();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Select(q => q.QuestionText),
            Is.EquivalentTo(new[] { "Question 1", "Question 2", "Question 3" }));
    }
}