using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.StudentCourses
{
    public class StudentCourseRepo(EduQuestContext context) : BaseRepo<int, StudentCourse>(context), IStudentCourseRepo
    {
        public override async Task<List<Entities.StudentCourse>> GetAll()
        {
            var courses = await context.StudentCourses.AsNoTracking().Include(sc => sc.Course).ToListAsync();

            return courses;
        }
    }
}
