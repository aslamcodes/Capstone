using EduQuest.Commons;
using EduQuest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduQuest.Features.Questions
{
    public class QuestionRepo(EduQuestContext context) : BaseRepo<int, Question>(context), IQuestionRepo
    {
        public async Task<List<Question>> GetQuestionsByContent(int contentId)
        {
            var questions = await context.Questions.Include(q => q.PostedBy).AsNoTracking().Where(question => question.ContentId == contentId).ToListAsync();

            return questions;
        }
    }
}
