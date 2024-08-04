using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Courses
{
    public class CourseRepository(EduQuestContext context) : BaseRepo<int, Course>(context), ICourseRepo
    {
        public override Task<List<Course>> GetAll()
        {
            return context.Courses.Include(c => c.Students).ToListAsync();
        }

        public async Task<List<Course>> GetBySearch(string query)
        {
            var results = await context.Courses
                .Where(item => item.CourseStatus == CourseStatusEnum.Live)
                .Where(item =>
                    (item.Name != null && item.Name.ToLower().Contains(query)) ||
                    (item.Description != null && item.Description.ToLower().Contains(query)))
                .Select(item => new
                {
                    Item = item,
                    RelevanceScore = (item.Name != null && item.Name.ToLower().Contains(query) ? 2 : 0)
                                     + (item.Description != null && item.Description.ToLower().Contains(query) ? 1 : 0)
                })
                .OrderByDescending(x => x.RelevanceScore)
                .ThenBy(x => x.Item.Name)
                .Select(x => x.Item)
                .Take(10)
                .ToListAsync();

            return results;
        }

        public async Task<List<Course>> GetByStatus(CourseStatusEnum status)
        {
            var courses = await context.Courses.Include(c => c.Educator)
                .Where(c => c.CourseStatus == status)
                .AsNoTracking()
                .ToListAsync();

            return courses;
        }
    }
}