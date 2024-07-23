using EduQuest.Commons;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.User;
using System.Security.Cryptography;
using System.Text;

namespace EduQuest.Features.Auth
{
    public class AuthService(IUserService userService, ITokenService tokenService, IRepository<int, User.User> userRepository) : IAuthService
    {

        public async Task<User.User> Login(AuthRequestDto request)
        {
            var userDB = await userService.GetByEmailAsync(request.Email);

            HMACSHA512 hMACSHA = new(userDB.PasswordHashKey);

            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            if (!userDB.IsPasswordCorrect(encrypterPass))
            {
                throw new InvalideCredsException();
            }


            return userDB;
        }

        public async Task<User.User> Register(RegisterRequestDto request)
        {
            HMACSHA512 hMACSHA = new();

            var existingUser = await userService.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException();
            }

            var user = new User.User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHashKey = hMACSHA.Key,
                Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
            };

            user = await userRepository.Add(user);

            return user;

        }
    }
}
