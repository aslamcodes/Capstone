namespace EduQuest.Features.User
{
    public interface IUserService
    {
        Task<User> GetByEmailAsync(string email);
    }
}
