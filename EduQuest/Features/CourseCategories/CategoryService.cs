using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.CourseCategories
{
    public class CategoryService(IRepository<int, CourseCategory> categoryRepo, IMapper mapper) : BaseService<CourseCategory, CourseCategoryDto>(categoryRepo, mapper), ICategoryService
    {

    }
}
