using EduQuest.Commons;
using EduQuest.Features.Sections;

namespace EduQuest.Features.Content
{
    public class Content : BaseEntity
    {
        public string Title { get; set; }
        public int SectionId { get; set; }
        public int OrderIndex { get; set; }
        public Section Section { get; set; }
        public ContentTypeEnum ContentType { get; set; }
    }
}
