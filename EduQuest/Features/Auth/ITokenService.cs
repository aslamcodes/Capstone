namespace EduQuest.Features.Auth
{
    public interface ITokenService
    {
        public string GenerateUserToken(Entities.User user);
    }
}
