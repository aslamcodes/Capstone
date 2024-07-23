namespace EduQuest.Features.Auth
{
    public interface ITokenService
    {
        public string GenerateUserToken(User.User user);
    }
}
