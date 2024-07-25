namespace EduQuest.Features.Auth.Exceptions
{
    public class UnAuthorisedUserExeception : Exception
    {
        public override string Message => "Unauthorized access";
    }
}
