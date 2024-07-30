using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Notes
{
    public class NotesRepo(EduQuestContext context) : BaseRepo<int, Note>(context), INotesRepo
    {
    }
}
