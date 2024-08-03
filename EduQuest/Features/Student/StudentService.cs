using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses;
using EduQuest.Features.Courses.Dto;

namespace EduQuest.Features.Student
{
    public class StudentService(ICourseService courseService, IRepository<int, StudentCourse> studentCourseRepo, IMapper mapper) : IStudentService
    {


        public async Task<List<CourseDTO>> GetHomeCourses(int studentId)
        {
            var courses = await courseService.GetAll();

            var recommendedCourses = courses.Where(c => c.CourseStatus == CourseStatusEnum.Live.ToString())
                                            .ToList();

            return recommendedCourses;
        }

        public async Task<List<CourseDTO>> GetRecommendedCourses(int userId)
        {
            // TODO: Implement a ML.net model
            // to recommend courses to students
            // based on their past performance
            // and interests
            var enrolledCourses = await courseService.GetCoursesForStudent(userId);

            var courses = await courseService.GetAll();

            var recommendedCourses = courses.Where(c => c.EducatorId != userId)
                                            .Where(c => c.CourseStatus == CourseStatusEnum.Live.ToString())
                                            .Where(c => !enrolledCourses.Any(ec => c.Id == ec.Id))
                                            .ToList();

            return recommendedCourses;
        }

        public async Task<UserOwnsDto> UserManagesCourse(int educatorId, int courseId)
        {
            var educatorCourses = await courseService.GetCoursesForEducator(educatorId);

            var userManagesCourse = educatorCourses.Any(c => c.Id == courseId);

            return new UserOwnsDto()
            {
                UserOwnsCourse = userManagesCourse,
                CourseId = courseId,
            };
        }

        public async Task<UserOwnsDto> UserOwnsCourse(int studentId, int courseId)
        {
            var enrolledCourses = await courseService.GetCoursesForStudent(studentId);

            var userOwnsCourse = enrolledCourses.Any(c => c.Id == courseId);

            return new UserOwnsDto
            {
                StudentId = studentId,
                CourseId = courseId,
                UserOwnsCourse = userOwnsCourse
            };
        }
    }
}