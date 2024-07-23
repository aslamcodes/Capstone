using EduQuest.Features.Auth.DTOS;

namespace EduQuest.Features.Auth
{
    public interface IAuthService
    {
        Task<User.User> Login(AuthRequestDto request);

        Task<User.User> Register(RegisterRequestDto request);
    }
}
