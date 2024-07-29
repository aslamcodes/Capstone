namespace EduQuest.Features.Users
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public override string Message => "User not found";
    }
}