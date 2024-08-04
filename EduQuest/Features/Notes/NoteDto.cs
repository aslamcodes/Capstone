using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Notes
{
    [ExcludeFromCodeCoverage]
    public class NoteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContentId { get; set; }

        [Required] public string NoteContent { get; set; }
    }
}