using EduQuest.Commons;
using EduQuest.Features.Content.Dto;

namespace EduQuest.Features.Content
{
    public interface IContentService : IBaseService<Content, ContentResponseDto>
    {
        Task<IEnumerable<ContentResponseDto>> GetContentBySection(int sectionId);
    }
}