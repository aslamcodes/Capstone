using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Articles
{
    public class ArticleService(IArticleRepo articleRepo, IMapper mapper) : BaseService<Article, ArticleDto>(articleRepo, mapper), IArticleService
    {
        public async Task<ArticleDto> GetByContentId(int contentId)
        {
            var article = await articleRepo.GetByContentId(contentId);

            return mapper.Map<ArticleDto>(article);
        }
    }
}
