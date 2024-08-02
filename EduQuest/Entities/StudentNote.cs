using System.Diagnostics.CodeAnalysis;
using EduQuest.Commons;

namespace EduQuest.Entities
{
    [ExcludeFromCodeCoverage]
    public class StudentNote : BaseEntity
    {
        public int StudentId { get; set; }
        public int ContentId { get; set; }
        public string Note { get; set; }

        public User Student { get; set; }
        public Content Content { get; set; }
    }
}
