namespace EduQuest.Features.Notes
{
    public class NoteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public string NoteContent { get; set; }
    }
}