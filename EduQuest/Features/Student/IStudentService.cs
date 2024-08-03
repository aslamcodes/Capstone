using EduQuest.Features.Courses.Dto;

namespace EduQuest.Features.Student
{
    public interface IStudentService
    {
        Task<List<CourseDTO>> GetHomeCourses(int studentId);
        Task<List<CourseDTO>> GetRecommendedCourses(int studentId);
        Task<UserOwnsDto> UserOwnsCourse(int studentId, int courseId);
        Task<UserOwnsDto> UserManagesCourse(int studentId, int courseId);
    }
}