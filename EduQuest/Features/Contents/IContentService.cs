using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Contents.Dto;

namespace EduQuest.Features.Contents
{
    public interface IContentService : IBaseService<Content, ContentDto>
    {
        Task DeleteBySection(int sectionId);
        Task<IEnumerable<ContentDto>> GetContentBySection(int sectionId);
    }
}