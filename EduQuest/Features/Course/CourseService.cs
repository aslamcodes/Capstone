using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Course.Dto;

namespace EduQuest.Features.Course
{
    public class CourseService(ICourseRepo courseRepo, IMapper mapper) : BaseService<Course, CourseDTO>(courseRepo, mapper), ICourseService
    {
        public async Task<List<Course>> GetCoursesForEducator(int educatorId)
        {
            var courses = await courseRepo.GetAll();

            return courses.Where(c => c.EducatorId == educatorId).ToList();
        }

        public async Task<List<Course>> GetCoursesForStudent(int studentId)
        {
            var courses = await courseRepo.GetAll();

            return courses.Where(c => c.Students.Any(s => s.Id == studentId)).ToList();
        }
    }
}
