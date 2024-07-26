using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class Video : BaseEntity
    {
        public int ContentId { get; set; }

        public int DurationHours { get; set; }

        public int DurationMinutes { get; set; }

        public int DurationSeconds { get; set; }

        public string Url { get; set; }

        public Content Content { get; set; }
    }
}
