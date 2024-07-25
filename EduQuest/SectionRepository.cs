using EduQuest.Commons;
using EduQuest.Features.Sections;

namespace EduQuest
{
    public class SectionRepository(EduQuestContext context) : BaseRepo<int, Section>(context);
}