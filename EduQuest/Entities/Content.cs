using EduQuest.Commons;
using EduQuest.Features.Contents;

namespace EduQuest.Entities
{
    public class Content : BaseEntity
    {
        public string Title { get; set; }
        public int SectionId { get; set; }
        public int OrderId { get; set; }
        public Section Section { get; set; }
        public ContentTypeEnum ContentType { get; set; }
        public Video Video { get; set; }

        public Article Article { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
