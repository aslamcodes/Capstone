using EduQuest.Entities;

namespace EduQuest.Features.Users
{
    public interface IUserService
    {
        Task<User> AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
    }
}
