namespace EduQuest.Features.Auth
{
    [Serializable]
    internal class InvalideCredsException : Exception
    {
        public override string Message => "Invalid Credentials";
    }
}