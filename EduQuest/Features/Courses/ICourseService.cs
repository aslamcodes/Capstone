using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses.Dto;
namespace EduQuest.Features.Courses
{
    public interface ICourseService : IBaseService<Course, CourseDTO>
    {
        Task<List<CourseDTO>> GetCoursesForStudent(int studentId);

        Task<List<CourseDTO>> GetCoursesForEducator(int educatorId);

        Task<CourseDTO> EnrollStudentIntoCourse(int studentId, int courseId);
    }
}
