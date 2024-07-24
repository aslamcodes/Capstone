using EduQuest.Commons;

namespace EduQuest.Features.Content
{
    public interface IContentRepo : IRepository<int, Content>
    {
        Task<List<Content>> GetContentsBySection(int sectionId);
    }
}
