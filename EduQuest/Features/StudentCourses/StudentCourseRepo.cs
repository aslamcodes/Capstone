using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.StudentCourses
{
    public class StudentCourseRepo(EduQuestContext context) : BaseRepo<int, Entities.StudentCourseRepo>(context)
    {
        public override async Task<List<Entities.StudentCourseRepo>> GetAll()
        {
            var courses = await context.StudentCourses.AsNoTracking().Include(sc => sc.Course).ToListAsync();

            return courses;
        }
    }
}
