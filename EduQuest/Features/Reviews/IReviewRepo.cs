using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Reviews
{
    public interface IReviewRepo : IRepository<int, Review>
    {
        Task<Review?> GetByUserAndCourse(int reviewedById, int courseId);
        Task<List<Review>> GetReviewsByCourse(int courseId);
    }
}