using System.Diagnostics.CodeAnalysis;
using EduQuest.Commons;

namespace EduQuest.Entities
{
    [ExcludeFromCodeCoverage]
    public class CourseSkill : BaseEntity
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int SkillId { get; set; }

        public Course Course { get; set; }

        public Skill Skill { get; set; }
    }
}
