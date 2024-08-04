using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Users
{
    [ExcludeFromCodeCoverage]
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsEducator { get; set; }

        public string Email { get; set; }

        public string ProfilePictureUrl { get; set; }
    }
}