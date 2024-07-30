using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Answers
{
    public class AnswersService(IAnswerRepo answerRepo, IMapper mapper) : BaseService<Answer, AnswerDto>(answerRepo, mapper), IAnswerService
    {
        public async Task<List<AnswerDto>> GetAnswersForQuestion(int questionId)
        {
            var answers = await answerRepo.GetAnswersByQuestion(questionId);

            return mapper.Map<List<AnswerDto>>(answers);
        }

    }
}
