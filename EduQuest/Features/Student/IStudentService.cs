using EduQuest.Features.Courses.Dto;

namespace EduQuest.Features.Student
{
    public interface IStudentService
    {
        Task<List<CourseDTO>> GetRecommendedCourses(int studentId);
    }
}