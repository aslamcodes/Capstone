using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Articles;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EduQuestTests.Articles
{
    public class ArticleRepoTests
    {
        private EduQuestContext _context;
        private ArticleRepo _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EduQuestContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EduQuestContext(options);
            _repo = new ArticleRepo(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Add_ShouldAddArticleToDatabase()
        {
            // Arrange
            var article = new Article { Id = 1, Title = "Test Article", Description = "Test Description", Body = "Test Body", ContentId = 1 };

            // Act
            var result = await _repo.Add(article);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("Test Article"));
            Assert.That(result.Description, Is.EqualTo("Test Description"));
            Assert.That(result.Body, Is.EqualTo("Test Body"));
            Assert.That(result.ContentId, Is.EqualTo(1));
            Assert.That(await _context.Articles.CountAsync(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetByKey_ShouldReturnCorrectArticle()
        {
            // Arrange
            var article = new Article { Id = 1, Title = "Test Article", Description = "Test Description", Body = "Test Body", ContentId = 1 };
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByKey(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("Test Article"));
            Assert.That(result.Description, Is.EqualTo("Test Description"));
            Assert.That(result.Body, Is.EqualTo("Test Body"));
            Assert.That(result.ContentId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllArticles()
        {
            // Arrange
            var articles = new[]
            {
                new Article { Id = 1, Title = "Article 1", Description = "Description 1", Body = "Body 1", ContentId = 1 },
                new Article { Id = 2, Title = "Article 2", Description = "Description 2", Body = "Body 2", ContentId = 2 },
                new Article { Id = 3, Title = "Article 3", Description = "Description 3", Body = "Body 3", ContentId = 3 }
            };
            await _context.Articles.AddRangeAsync(articles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.Select(a => a.Title), Is.EquivalentTo(new[] { "Article 1", "Article 2", "Article 3" }));
        }

        [Test]
        public async Task Update_ShouldUpdateExistingArticle()
        {
            // Arrange
            var article = new Article { Id = 1, Title = "Original Title", Description = "Original Description", Body = "Original Body", ContentId = 1 };
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            article.Title = "Updated Title";
            article.Description = "Updated Description";
            article.Body = "Updated Body";

            // Act
            var result = await _repo.Update(article);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Updated Title"));
            Assert.That(result.Description, Is.EqualTo("Updated Description"));
            Assert.That(result.Body, Is.EqualTo("Updated Body"));
            var updatedArticle = await _context.Articles.FindAsync(1);
            Assert.That(updatedArticle.Title, Is.EqualTo("Updated Title"));
            Assert.That(updatedArticle.Description, Is.EqualTo("Updated Description"));
            Assert.That(updatedArticle.Body, Is.EqualTo("Updated Body"));
        }

        [Test]
        public async Task Delete_ShouldRemoveArticleFromDatabase()
        {
            // Arrange
            var article = new Article { Id = 1, Title = "Test Article", Description = "Test Description", Body = "Test Body", ContentId = 1 };
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(await _context.Articles.CountAsync(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetByContentId_ShouldReturnCorrectArticle()
        {
            // Arrange
            var articles = new[]
            {
                new Article { Id = 1, Title = "Article 1", Description = "Description 1", Body = "Body 1", ContentId = 10 },
                new Article { Id = 2, Title = "Article 2", Description = "Description 2", Body = "Body 2", ContentId = 20 },
                new Article { Id = 3, Title = "Article 3", Description = "Description 3", Body = "Body 3", ContentId = 30 }
            };
            await _context.Articles.AddRangeAsync(articles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByContentId(20);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(2));
            Assert.That(result.Title, Is.EqualTo("Article 2"));
            Assert.That(result.Description, Is.EqualTo("Description 2"));
            Assert.That(result.Body, Is.EqualTo("Body 2"));
            Assert.That(result.ContentId, Is.EqualTo(20));
        }

        [Test]
        public void GetByContentId_ShouldThrowExceptionWhenArticleNotFound()
        {
            // Arrange
            var articles = new[]
            {
                new Article { Id = 1, Title = "Article 1", Description = "Description 1", Body = "Body 1", ContentId = 10 },
                new Article { Id = 2, Title = "Article 2", Description = "Description 2", Body = "Body 2", ContentId = 20 }
            };
            _context.Articles.AddRange(articles);
            _context.SaveChanges();

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repo.GetByContentId(30));
        }
    }
}