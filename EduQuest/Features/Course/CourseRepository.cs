using EduQuest.Commons;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Course
{
    public class CourseRepository(EduQuestContext context) : BaseRepo<int, Course>(context), ICourseRepo
    {
        public override Task<List<Course>> GetAll()
        {
            return context.Courses.Include(c => c.Students).ToListAsync();
        }

    }
}
