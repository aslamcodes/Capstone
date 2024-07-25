using EduQuest.Commons;
using EduQuest.Features.Content.Dto;

namespace EduQuest.Features.Content
{
    public interface IContentService : IBaseService<Content, ContentDto>
    {
        Task<IEnumerable<ContentDto>> GetContentBySection(int sectionId);
    }
}