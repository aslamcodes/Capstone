using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Notes
{
    public class NotesService(INotesRepo notesRepo, IMapper mapper) : BaseService<Note, NoteDto>(notesRepo, mapper), INotesService
    {
        public async Task<NoteDto?> GetNotesForContent(int contentId)
        {
            var notes = await notesRepo.GetAll();

            var note = (notes.FirstOrDefault(n => n.ContentId == contentId));

            return note == null ? null : mapper.Map<NoteDto>(note);
        }
    }
}
