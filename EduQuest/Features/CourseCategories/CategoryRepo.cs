using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.CourseCategories
{
    public class CategoryRepo(EduQuestContext context) : BaseRepo<int, CourseCategory>(context)
    {

    }
}
