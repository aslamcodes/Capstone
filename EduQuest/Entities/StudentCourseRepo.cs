using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class StudentCourseRepo : BaseEntity
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public User Student { get; set; }

        public Course Course { get; set; }
    }
}
