using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class Article : BaseEntity
    {
        public int ContentId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public Content Content { get; set; }
    }
}
