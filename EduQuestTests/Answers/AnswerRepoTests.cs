using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Answers;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.Answers;

    [TestFixture]
    public class AnswerRepoTests
    {
        private EduQuestContext _context;
        private AnswerRepo _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EduQuestContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EduQuestContext(options);
            _repo = new AnswerRepo(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Add_ShouldAddAnswerToDatabase()
        {
            // Arrange
            var answer = new Answer() { Id = 1, AnswerText = "Test Answer", QuestionId = 1 };

            // Act
            var result = await _repo.Add(answer);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(await _context.Answers.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetByKey_ShouldReturnCorrectAnswer()
        {
            // Arrange
            var answer = new Answer { Id = 1, AnswerText = "Test Answer", QuestionId = 1 };
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByKey(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.AnswerText, Is.EqualTo("Test Answer"));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllAnswers()
        {
            // Arrange
            var answers = new List<Answer>
            {
                new Answer { Id = 1, AnswerText = "Answer 1", QuestionId = 1 },
                new Answer { Id = 2, AnswerText = "Answer 2", QuestionId = 1 },
                new Answer { Id = 3, AnswerText = "Answer 3", QuestionId = 2 }
            };
            await _context.Answers.AddRangeAsync(answers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task Update_ShouldUpdateExistingAnswer()
        {
            // Arrange
            var answer = new Answer { Id = 1, AnswerText = "Original Answer", QuestionId = 1 };
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            answer.AnswerText = "Updated Answer";

            // Act
            var result = await _repo.Update(answer);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AnswerText, Is.EqualTo("Updated Answer"));
            var updatedAnswer = await _context.Answers.FindAsync(1);
            Assert.That(updatedAnswer.AnswerText, Is.EqualTo("Updated Answer"));
        }

        [Test]
        public async Task Delete_ShouldRemoveAnswerFromDatabase()
        {
            // Arrange
            var answer = new Answer { Id = 1, AnswerText = "Test Answer", QuestionId = 1 };
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(await _context.Answers.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetAnswersByQuestion_ShouldReturnCorrectAnswers()
        {
            // Arrange
            var answers = new List<Answer>
            {
                new Answer { Id = 1, AnswerText = "Answer 1", QuestionId = 1 },
                new Answer { Id = 2, AnswerText = "Answer 2", QuestionId = 1 },
                new Answer { Id = 3, AnswerText = "Answer 3", QuestionId = 2 }
            };
            await _context.Answers.AddRangeAsync(answers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAnswersByQuestion(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(a => a.QuestionId == 1), Is.True);
        }
    }