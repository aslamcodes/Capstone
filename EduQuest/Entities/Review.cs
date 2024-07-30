using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class Review : BaseEntity
    {
        public int CourseId { get; set; }

        public int Rating { get; set; }

        public string ReviewText { get; set; }

        public int ReviewedById { get; set; }

        public DateTime ReviewedOn { get; set; } = DateTime.Now;

        public Course Course { get; set; }

        public User ReviewedBy { get; set; }
    }
}
