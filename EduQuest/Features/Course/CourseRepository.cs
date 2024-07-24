using EduQuest.Commons;

namespace EduQuest.Features.Course
{
    public class CourseRepository(EduQuestContext context) : BaseRepo<int, Course>(context);

}
