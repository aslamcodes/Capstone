using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Notes
{
    public interface INotesService : IBaseService<Note, NoteDto>
    {
        Task<NoteDto?> GetNotesForContent(int contentId);
    }
}
