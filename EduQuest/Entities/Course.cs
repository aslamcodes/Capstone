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

        public int CourseCategoryId { get; set; }

        public string? CourseObjective { get; set; }
        public string? TargetAudience { get; set; }

        public string? Prerequisites { get; set; }

        public string? CourseThumbnailPicture { get; set; }

        public CourseCategory CourseCategory { get; set; }

        public CourseLevelEnum Level { get; set; }

        public CourseStatusEnum CourseStatus { get; set; } = CourseStatusEnum.Draft;

        public User Educator { get; set; }

        public ICollection<User> Students { get; set; }

        public IEnumerable<Section> Sections { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        public IEnumerable<Skill> Skills { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public override string ToString()
        {
            return Name
                + " "
                + Description
                + " "
                + CourseObjective
                + " "
                + TargetAudience
                + " "
                + Prerequisites
                + " "
                + CourseCategory.ToString()
                + " "
                + Level.ToString();

        }
    }
}
