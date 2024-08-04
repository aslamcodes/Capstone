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

        Task<CourseDTO> SetCourseLive(int courseId);
        Task<CourseDTO> SetCourseUnderReview(int courseId);

        Task<ValidityResponseDto> GetCourseValidity(int courseId);
        Task<List<CourseDTO>> SearchCourse(string query);
        Task<List<CourseDTO>> GetCoursesByStatus(CourseStatusEnum status);
        Task<CourseDTO> SetCourseOutdated(int courseId);
        Task<CourseDTO> SetCourseProfile(int courseId, string fileUrl);
    }
}
