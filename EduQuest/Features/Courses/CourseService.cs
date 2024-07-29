using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Courses.Dto;

namespace EduQuest.Features.Courses

{
    public class CourseService(ICourseRepo courseRepo, IRepository<int, User> userRepo, IRepository<int, StudentCourse> studentCourse, IMapper mapper) : BaseService<Course, CourseDTO>(courseRepo, mapper), ICourseService
    {
        public async Task<CourseDTO> EnrollStudentIntoCourse(int studentId, int courseId)
        {
            var course = await courseRepo.GetByKey(courseId);

            await studentCourse.Add(new StudentCourse { StudentId = studentId, CourseId = courseId });

            return mapper.Map<CourseDTO>(course);
        }

        public async Task<List<CourseDTO>> GetCoursesForEducator(int educatorId)
        {
            var courses = await courseRepo.GetAll();

            return courses.Where(c => c.EducatorId == educatorId).Select(mapper.Map<CourseDTO>).ToList();
        }

        public async Task<List<CourseDTO>> GetCoursesForStudent(int studentId)
        {
            var courses = (await studentCourse.GetAll()).Where(sc => sc.StudentId == studentId);

            return courses.Select(c => mapper.Map<CourseDTO>(c.Course)).ToList();
        }
    }
}
