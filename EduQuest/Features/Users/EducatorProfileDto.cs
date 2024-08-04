using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Users
{
    [ExcludeFromCodeCoverage]
    public class EducatorProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // public string? Description { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}