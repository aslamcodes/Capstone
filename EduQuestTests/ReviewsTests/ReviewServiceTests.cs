using NUnit.Framework;
using Moq;
using EduQuest.Entities;
using EduQuest.Features.Reviews;
using AutoMapper;

namespace EduQuestTests.ReviewsTests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private Mock<IReviewRepo> _mockReviewRepo;
        private IMapper _mapper;
        private ReviewService _reviewService;

        [SetUp]
        public void Setup()
        {
            _mockReviewRepo = new Mock<IReviewRepo>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Review, ReviewDto>();
                cfg.CreateMap<ReviewDto, Review>();
            });
            _mapper = config.CreateMapper();

            _reviewService = new ReviewService(_mockReviewRepo.Object, _mapper);
        }

        [Test]
        public async Task GetByUserAndCourse_ShouldReturnReviewForGivenUserAndCourse()
        {
            // Arrange
            int reviewedById = 1;
            int courseId = 1;
            var review = new Review
            {
                Id = 1,
                CourseId = courseId,
                ReviewedById = reviewedById,
                ReviewText = "Great course!"
            };

            _mockReviewRepo.Setup(repo => repo.GetByUserAndCourse(reviewedById, courseId)).ReturnsAsync(review);

            // Act
            var result = await _reviewService.GetByUserAndCourse(reviewedById, courseId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(review.ReviewText, result.ReviewText);
            Assert.AreEqual(review.CourseId, result.CourseId);
            Assert.AreEqual(review.ReviewedById, result.ReviewedById);
        }

        [Test]
        public async Task GetReviewsByCourse_ShouldReturnAllReviewsForGivenCourse()
        {
            // Arrange
            int courseId = 1;
            var reviews = new List<Review>
            {
                new Review { Id = 1, CourseId = courseId, ReviewedById = 1, ReviewText  = "Excellent!" },
                new Review { Id = 2, CourseId = courseId, ReviewedById = 2, ReviewText = "Very good!" }
            };

            _mockReviewRepo.Setup(repo => repo.GetReviewsByCourse(courseId)).ReturnsAsync(reviews);

            // Act
            var result = await _reviewService.GetReviewsByCourse(courseId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Excellent!", result[0].ReviewText);
            Assert.AreEqual("Very good!", result[1].ReviewText);
        }
    }
}
