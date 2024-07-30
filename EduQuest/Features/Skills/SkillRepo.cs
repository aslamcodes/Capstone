﻿using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Skills
{
    public class SkillRepo(EduQuestContext context) : BaseRepo<int, Skill>(context), ISkillRepo
    {
    }
}
