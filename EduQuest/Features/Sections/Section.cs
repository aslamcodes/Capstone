using EduQuest.Commons;

namespace EduQuest.Features.Sections
{
    public class Section : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public int OrderId { get; set; }
        public IEnumerable<Content.Content> Contents { get; set; }
        public Course.Course Course { get; set; }
    }
}


