namespace EduQuest.Features.User
{
    public interface IUserService
    {
        Task<User> AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
    }
}
