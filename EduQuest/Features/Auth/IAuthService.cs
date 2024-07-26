using EduQuest.Features.Auth.DTOS;

namespace EduQuest.Features.Auth
{
    public interface IAuthService
    {
        Task<Entities.User> Login(AuthRequestDto request);

        Task<Entities.User> Register(RegisterRequestDto request);
    }
}
