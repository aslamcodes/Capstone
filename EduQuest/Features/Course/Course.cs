using EduQuest.Commons;

namespace EduQuest.Features.Course
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int EducatorId { get; set; }

        public float Price { get; set; }

        public CourseLevelEnum Level { get; set; }

        public User.User Educator { get; set; }

        public IEnumerable<User.User> Students { get; set; }

        public IEnumerable<Sections.Section> Sections { get; set; }
    }
}
