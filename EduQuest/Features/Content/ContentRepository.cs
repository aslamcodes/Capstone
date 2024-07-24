using EduQuest.Commons;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Content
{
    public class ContentRepository(EduQuestContext context) : BaseRepo<int, Content>(context), IContentRepo
    {
        public async Task<List<Content>> GetContentsBySection(int sectionId)
        {
            var contents = await context.Contents.Where(c => c.SectionId == sectionId).ToListAsync();

            return contents;
        }
    }
}
