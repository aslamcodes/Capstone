using System.Diagnostics.CodeAnalysis;
using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Skills
{
    [ExcludeFromCodeCoverage]
    public class SkillRepo(EduQuestContext context) : BaseRepo<int, Skill>(context), ISkillRepo
    {
    }
}
