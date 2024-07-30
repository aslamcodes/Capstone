using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Reviews
{
    public class ReviewService(IReviewRepo repo, IMapper mapper) : BaseService<Review, ReviewDto>(repo, mapper), IReviewService
    {
        public async Task<ReviewDto> GetByUserAndCourse(int reviewedById, int courseId)
        {
            var review = await repo.GetByUserAndCourse(reviewedById, courseId);

            return mapper.Map<ReviewDto>(review);
        }

        public async Task<List<ReviewDto>> GetReviewsByCourse(int courseId)
        {
            var reviews = await repo.GetReviewsByCourse(courseId);

            return reviews.ConvertAll(mapper.Map<ReviewDto>);
        }
    }
}
