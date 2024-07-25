using EduQuest.Commons;

namespace EduQuest.Features.Sections
{
    public interface ISectionRepo : IRepository<int, Section>
    {
        Task<IList<Section>> DeleteByCourse(int courseId);
    }
}