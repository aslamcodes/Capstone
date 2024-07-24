using EduQuest.Commons;

namespace EduQuest.Features.Course
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int EducatorId { get; set; }

        public float Price { get; set; }

        public CourseLevel Level { get; set; }
    }
}
