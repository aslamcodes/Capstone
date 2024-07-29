using EduQuest.Commons;
using EduQuest.Entities;

namespace EduQuest.Features.Users
{
    public class UserService(IRepository<int, User> userRepository) : IUserService
    {
        public async Task<User> AddAsync(User user)
        {
            var newUser = await userRepository.Add(user);

            return newUser;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var users = await userRepository.GetAll();

            var user = users.Find(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            return user ?? throw new UserNotFoundException();
        }

    }
}
