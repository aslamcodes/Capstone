using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class Section : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public int OrderId { get; set; }
        public IEnumerable<Content> Contents { get; set; }
        public Course Course { get; set; }
    }
}


