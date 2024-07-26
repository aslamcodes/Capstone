using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Sections
{
    public class SectionRepository(EduQuestContext context) : BaseRepo<int, Section>(context), ISectionRepo
    {
        public async Task<IList<Section>> DeleteByCourse(int courseId)
        {
            var sectionsToRemove = await context.Sections.Where(s => s.CourseId == courseId).ToListAsync();

            context.RemoveRange(sectionsToRemove);

            await context.SaveChangesAsync();

            return sectionsToRemove;
        }
    }
}