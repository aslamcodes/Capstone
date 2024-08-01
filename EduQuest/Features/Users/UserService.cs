using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Questions;

namespace EduQuest.Features.Users
{
    public class UserService(IRepository<int, User> userRepository, IMapper mapper) : BaseService<User, UserProfileDto>(userRepository, mapper), IUserService
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

        public async Task<UserProfileDto> MakeEducator(int userId)
        {
            var user = await userRepository.GetByKey(userId);

            user.IsEducator = true;

            var updatedUser = await userRepository.Update(user);

            return mapper.Map<UserProfileDto>(updatedUser);
        }

        public async Task<UserProfileDto> UpdateProfile(UserProfileDto updateEntity)
        {
            var user = await userRepository.GetByKey(updateEntity.Id);

            user.FirstName = updateEntity.FirstName;
            user.LastName = updateEntity.LastName;
            user.Email = updateEntity.Email;
            user.ProfilePictureUrl = updateEntity.ProfilePictureUrl;

            var updatedUser = await userRepository.Update(user);

            return mapper.Map<UserProfileDto>(updatedUser);

        }

        public async Task<UserProfileDto> UpdateProfileEntries(UserProfileUpdateDto updateEntity)
        {
            var user = await userRepository.GetByKey(updateEntity.Id);

            user.FirstName = updateEntity.FirstName;
            user.LastName = updateEntity.LastName;
            user.Email = updateEntity.Email;

            var updatedUser = await userRepository.Update(user);

            return mapper.Map<UserProfileDto>(updatedUser);
        }
    }
}
