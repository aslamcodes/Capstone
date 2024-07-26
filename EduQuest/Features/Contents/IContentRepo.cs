using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Contents
{
    public interface IContentRepo : IRepository<int, Content>
    {
        Task DeleteContentsBySection(int sectionId);
        Task<List<Content>> GetContentsBySection(int sectionId);
    }
}
