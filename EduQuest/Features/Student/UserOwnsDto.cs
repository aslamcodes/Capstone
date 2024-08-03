using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Student
{
    [ExcludeFromCodeCoverage]
    public class UserOwnsDto
    {
        public bool UserOwnsCourse { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}