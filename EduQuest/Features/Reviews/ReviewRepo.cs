using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Reviews
{
    public class ReviewRepo(EduQuestContext context) : BaseRepo<int, Review>(context), IReviewRepo
    {
        public async Task<Review?> GetByUserAndCourse(int reviewedById, int courseId)
        {
            var review = await context.Reviews.FirstOrDefaultAsync(r => r.ReviewedById == reviewedById && r.CourseId == courseId);

            return review;
        }

        public async Task<List<Review>> GetReviewsByCourse(int courseId)
        {
            var reviews = await context.Reviews.Include(r => r.ReviewedBy).Where(r => r.CourseId == courseId).ToListAsync();

            return reviews;
        }
    }
}
