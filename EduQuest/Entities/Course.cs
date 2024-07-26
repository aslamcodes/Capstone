using EduQuest.Commons;
using EduQuest.Features.Courses;

namespace EduQuest.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int EducatorId { get; set; }

        public float Price { get; set; }

        public CourseLevelEnum Level { get; set; }

        public User Educator { get; set; }

        public ICollection<User> Students { get; set; }

        public IEnumerable<Section> Sections { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
