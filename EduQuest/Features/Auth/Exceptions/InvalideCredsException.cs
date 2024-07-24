namespace EduQuest.Features.Auth
{
    [Serializable]
    public class InvalideCredsException : Exception
    {
        public override string Message => "Invalid Credentials";
    }
}