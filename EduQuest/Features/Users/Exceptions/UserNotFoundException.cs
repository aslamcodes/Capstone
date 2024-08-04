using System.Diagnostics.CodeAnalysis;

namespace EduQuest.Features.Users
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public override string Message => "User not found";
    }
}