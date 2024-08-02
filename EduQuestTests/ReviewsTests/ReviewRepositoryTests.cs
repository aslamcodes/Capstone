using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Reviews;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.ReviewsTests;

[TestFixture]
public class ReviewRepoTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _reviewRepo = new ReviewRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private ReviewRepo _reviewRepo;

    [Test]
    public async Task GetByUserAndCourse_ReturnsCorrectReview()
    {
        // Arrange
        var review = new Review { Id = 1, ReviewedById = 1, CourseId = 1, Rating = 5, ReviewText = "Great course!" };
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Act
        var result = await _reviewRepo.GetByUserAndCourse(1, 1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Rating, Is.EqualTo(5));
        Assert.That(result.ReviewText, Is.EqualTo("Great course!"));
    }

    [Test]
    public async Task GetByUserAndCourse_ReturnsNullForNonexistentReview()
    {
        // Act
        var result = await _reviewRepo.GetByUserAndCourse(1, 1);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetReviewsByCourse_ReturnsCorrectReviews()
    {
        // Arrange
        var user1 = new User { Id = 1, FirstName = "John",  LastName = "Test", Email = "tes1t@test.com", Password = [],
            PasswordHashKey = [], ProfilePictureUrl = "1231", };
        var user2 = new User { Id = 2, FirstName = "Jane" , LastName = "Test", Email = "tes12t@test.com", Password = [],
            PasswordHashKey = [], ProfilePictureUrl = "1231"};
        _context.Users.AddRange(user1, user2);

        var reviews = new[]
        {
            new Review
            {
                Id = 1, ReviewedById = 1, CourseId = 1, Rating = 5, ReviewText = "Great course!", ReviewedBy = user1
            },
            new Review
            {
                Id = 2, ReviewedById = 2, CourseId = 1, Rating = 4, ReviewText = "Good course!", ReviewedBy = user2
            },
            new Review
            {
                Id = 3, ReviewedById = 1, CourseId = 2, Rating = 3, ReviewText = "Average course!", ReviewedBy = user1
            }
        };
        _context.Reviews.AddRange(reviews);
        await _context.SaveChangesAsync();

        // Act
        var result = await _reviewRepo.GetReviewsByCourse(1);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(r => r.Rating), Is.EquivalentTo(new[] { 5, 4 }));
        Assert.That(result.All(r => r.ReviewedBy != null), Is.True);
        Assert.That(result.Select(r => r.ReviewedBy.FirstName), Is.EquivalentTo(new[] { "John", "Jane" }));
    }

    [Test]
    public async Task Add_AddsReviewToDatabase()
    {
        // Arrange
        var review = new Review { ReviewedById = 1, CourseId = 1, Rating = 5, ReviewText = "Excellent course!" };

        // Act
        var result = await _reviewRepo.Add(review);

        // Assert
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.Reviews.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ReturnsCorrectReview()
    {
        // Arrange
        var review = new Review { Id = 1, ReviewedById = 1, CourseId = 1, Rating = 4, ReviewText = "Good course!" };
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Act
        var result = await _reviewRepo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Rating, Is.EqualTo(4));
        Assert.That(result.ReviewText, Is.EqualTo("Good course!"));
    }

    [Test]
    public async Task Update_UpdatesReviewInDatabase()
    {
        // Arrange
        var review = new Review { Id = 1, ReviewedById = 1, CourseId = 1, Rating = 3, ReviewText = "Average course" };
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Act
        review.Rating = 4;
        review.ReviewText = "Good course after updates";
        var result = await _reviewRepo.Update(review);

        // Assert
        Assert.That(result.Rating, Is.EqualTo(4));
        Assert.That(result.ReviewText, Is.EqualTo("Good course after updates"));
        var updatedReview = await _context.Reviews.FindAsync(1);
        Assert.That(updatedReview.Rating, Is.EqualTo(4));
        Assert.That(updatedReview.ReviewText, Is.EqualTo("Good course after updates"));
    }

    [Test]
    public async Task Delete_RemovesReviewFromDatabase()
    {
        // Arrange
        var review = new Review
            { Id = 1, ReviewedById = 1, CourseId = 1, Rating = 5, ReviewText = "Excellent course!" };
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Act
        await _reviewRepo.Delete(1);

        // Assert
        Assert.That(_context.Reviews.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetAll_ReturnsAllReviews()
    {
        // Arrange
        var reviews = new[]
        {
            new Review { Id = 1, ReviewedById = 1, CourseId = 1, Rating = 5, ReviewText = "Excellent!" },
            new Review { Id = 2, ReviewedById = 2, CourseId = 1, Rating = 4, ReviewText = "Good!" },
            new Review { Id = 3, ReviewedById = 3, CourseId = 2, Rating = 3, ReviewText = "Average" }
        };
        _context.Reviews.AddRange(reviews);
        await _context.SaveChangesAsync();

        // Act
        var result = await _reviewRepo.GetAll();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result.Select(r => r.Rating), Is.EquivalentTo(new[] { 5, 4, 3 }));
    }
}