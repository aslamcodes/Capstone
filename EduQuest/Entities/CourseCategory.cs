using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class CourseCategory : BaseEntity
    {


        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
