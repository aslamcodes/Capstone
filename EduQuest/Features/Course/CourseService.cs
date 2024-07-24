using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Course.Dto;

namespace EduQuest.Features.Course
{
    public class CourseService(IRepository<int, Course> courseRepo, IMapper mapper) : BaseService<Course, CourseDTO>(courseRepo, mapper), ICourseService
    {

    }
}
