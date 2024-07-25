using EduQuest.Commons;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Content
{
    public class ContentRepository(EduQuestContext context) : BaseRepo<int, Content>(context), IContentRepo
    {
        public async Task DeleteContentsBySection(int sectionId)
        {
            var contentsToDelete = await context.Contents
                                                .Where(c => c.SectionId == sectionId)
                                                .ToListAsync();

            context.Contents.RemoveRange(contentsToDelete);

            await context.SaveChangesAsync();
        }

        public async Task<List<Content>> GetContentsBySection(int sectionId)
        {
            var contents = await context.Contents.Where(c => c.SectionId == sectionId).ToListAsync();

            return contents;
        }


    }
}
