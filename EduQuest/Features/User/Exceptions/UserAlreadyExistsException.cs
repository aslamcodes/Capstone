namespace EduQuest.Features.Auth
{
    [Serializable]
    internal class UserAlreadyExistsException : Exception
    {
        public override string Message => "User Already Exists";

    }
}