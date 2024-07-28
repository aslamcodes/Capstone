using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Contents
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
            var contents = await context.Contents.AsNoTracking().Include(c => c.Video).Where(c => c.SectionId == sectionId).ToListAsync();

            return contents;
        }

        public override async Task<Content> GetByKey(int key)
        {
            var content = await context.Contents.AsNoTracking().Include(c => c.Video).Include(c => c.Article).FirstOrDefaultAsync(c => c.Id == key);

            return content ?? throw new EntityNotFoundException("Content not found");
        }
    }
}
