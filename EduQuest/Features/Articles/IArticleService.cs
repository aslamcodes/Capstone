using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Articles
{
    public interface IArticleService : IBaseService<Article, ArticleDto>
    {
        Task<ArticleDto> GetByContentId(int contentId);

    }
}
