using EduQuest.Commons;

namespace EduQuest.Entities
{
    public class Note : BaseEntity
    {
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public string NoteContent { get; set; }
        public User User { get; set; }
        public Content Content { get; set; }
    }
}
