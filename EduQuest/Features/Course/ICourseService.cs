using EduQuest.Commons;
using EduQuest.Features.Course.Dto;

namespace EduQuest.Features.Course
{
    public interface ICourseService : IBaseService<Course, CourseDTO>
    {
        Task<List<Course>> GetCoursesForStudent(int studentId);

        Task<List<Course>> GetCoursesForEducator(int educatorId);
    }
}
