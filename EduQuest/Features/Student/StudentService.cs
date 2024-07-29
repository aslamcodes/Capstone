using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;

namespace EduQuest.Features.Student
{
    public class StudentService(ICourseService courseService) : IStudentService
    {
        public async Task<List<CourseDTO>> GetRecommendedCourses(int userId)
        {
            // TODO: Implement a ML.net model
            // to recommend courses to students
            // based on their past performance
            // and interests
            var enrolledCourses = await courseService.GetCoursesForStudent(userId);

            var courses = await courseService.GetAll();

            return courses.Where(c => !enrolledCourses.Contains(c)).Take(10).ToList();
        }
    }
}