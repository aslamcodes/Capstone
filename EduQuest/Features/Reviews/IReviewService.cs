using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Reviews
{
    public interface IReviewService : IBaseService<Review, ReviewDto>
    {
        Task<ReviewDto> GetByUserAndCourse(int reviewedById, int courseId);
        Task<List<ReviewDto>> GetReviewsByCourse(int courseId);
    }
}
