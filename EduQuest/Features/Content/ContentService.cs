
using AutoMapper;
using EduQuest.Commons;
using EduQuest.Features.Content.Dto;

namespace EduQuest.Features.Content
{
    public class ContentService(IContentRepo contentRepo, IMapper mapper) : BaseService<Content, ContentDto>(contentRepo, mapper), IContentService
    {
        public async Task DeleteBySection(int sectionId)
        {
            await contentRepo.DeleteContentsBySection(sectionId);
        }

        public async Task<IEnumerable<ContentDto>> GetContentBySection(int sectionId)
        {
            var contents = await contentRepo.GetContentsBySection(sectionId);

            return contents.ConvertAll(mapper.Map<ContentDto>);
        }


    }
}
