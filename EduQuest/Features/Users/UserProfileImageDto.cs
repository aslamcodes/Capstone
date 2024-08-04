using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Users
{
    [ExcludeFromCodeCoverage]
    public class UserProfileImageDto
    {
        public IFormFile File { get; set; }
    }
}