namespace EduQuest.Features.Auth
{
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        public override string Message => "User Already Exists";

    }
}