using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.StudentCourses
{
    public class StudentCourseRepo(EduQuestContext context) : BaseRepo<int, StudentCourse>(context)
    {
    }
}
