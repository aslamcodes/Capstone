
using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Content.Dto;

namespace EduQuest.Features.Content
{
    public class ContentService(IContentRepo contentRepo, IMapper mapper) : BaseService<Content, ContentResponseDto>(contentRepo, mapper), IContentService
    {
        public async Task<IEnumerable<ContentResponseDto>> GetContentBySection(int sectionId)
        {
            var contents = await contentRepo.GetContentsBySection(sectionId);

            return contents.ConvertAll(mapper.Map<ContentResponseDto>);
        }
    }
}
