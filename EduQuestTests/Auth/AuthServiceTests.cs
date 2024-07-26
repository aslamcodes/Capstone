using EduQuest.Commons;
using EduQuest.Features.Auth;
using EduQuest.Features.Auth.DTOS;
using EduQuest.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EduQuestTests.Auth
{
    class AuthServiceTests
    {
        private ITokenService TokenService;
        private IAuthService AuthService;
        private EduQuestContext context;

        private EduQuestContext GetContext()
        {
            return context;
        }

        private void SetContext(EduQuestContext value)
        {
            context = value;
        }

        [SetUp]
        public void Setup()
        {
            Mock<IConfigurationSection> configurationJWTSection = new();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");

            Mock<IConfigurationSection> congigTokenSection = new();
            congigTokenSection.Setup(x => x.GetSection("key")).Returns(configurationJWTSection.Object);

            Mock<IConfiguration> mockConfig = new();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);

            var options = new DbContextOptionsBuilder<EduQuestContext>()
               .UseInMemoryDatabase(databaseName: "EduQuest")
               .Options;

            SetContext(new EduQuestContext(options));
            GetContext().Database.EnsureCreated();


            TokenService = new TokenService(mockConfig.Object);
            AuthService = new AuthService(new UserService(new UserRepo(GetContext())), TokenService);
        }

        [Test]
        public async Task AuthUserTest()
        {
            #region Arrange
            RegisterRequestDto registerDto = new()
            {
                Email = "johsa@gmail.com",
                FirstName = "joh",
                Password = "1234",
                LastName = "sa"
            };

            AuthRequestDto loginRequest = new()
            {
                Email = "johsa@gmail.com",
                Password = "1234"
            };
            #endregion

            #region Act
            var registerResult = await AuthService.Register(registerDto);
            var loginResult = await AuthService.Login(loginRequest);
            #endregion

            #region Assert

            Assert.Multiple(() =>
            {

                Assert.That(loginResult, Is.Not.Null);
                Assert.That(registerResult, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(registerResult.FirstName, Is.Not.Null);
                Assert.That(loginResult.Email, Is.Not.Null);
            });


            #endregion
        }

        [Test]
        public async Task AuthUseFailTest()
        {
            #region Arrange
            RegisterRequestDto registerDto = new()
            {
                Email = "johsa@gmail.com",
                FirstName = "joh",
                Password = "1234",
                LastName = "sa"
            };

            #endregion

            #region Act
            var registerResult = await AuthService.Register(registerDto);
            #endregion

            #region Assert

            Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await AuthService.Register(registerDto));

            #endregion
        }


        [Test]
        public async Task AuthUserTestFail()
        {
            #region Arrange
            RegisterRequestDto registerDto = new()
            {
                Email = "johsa@gmail.com",
                FirstName = "joh",
                Password = "1234",
                LastName = "sa"
            };

            AuthRequestDto loginRequest = new()
            {
                Email = "johsa@gmail.com",
                Password = "12314"
            };
            #endregion

            #region Act
            var registerResult = await AuthService.Register(registerDto);

            #endregion

            #region Assert


            Assert.ThrowsAsync<InvalideCredsException>(async () => await AuthService.Login(loginRequest));
            #endregion
        }

    }
}
