using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Articles
{
    public interface IArticleRepo : IRepository<int, Article>
    {
        Task<Article> GetByContentId(int contentId);
    }
}
