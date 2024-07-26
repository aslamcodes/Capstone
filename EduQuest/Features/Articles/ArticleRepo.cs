using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Articles
{
    public class ArticleRepo(EduQuestContext context) : BaseRepo<int, Article>(context), IArticleRepo
    {
        public async Task<Article> GetByContentId(int contentId)
        {
            var article = await context.Articles.FirstOrDefaultAsync(a => a.ContentId == contentId) ?? throw new EntityNotFoundException("Article not found for the content");

            return article;
        }
    }
}
