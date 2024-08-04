using AutoMapper;
using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Questions;
using EduQuest.Features.Users;
using Moq;

namespace EduQuestTests.UserTests;

[TestFixture]
public class UserServiceTests
{
    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IRepository<int, User>>();
        _mockMapper = new Mock<IMapper>();
        _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object);
    }

    private Mock<IRepository<int, User>> _mockUserRepository;
    private Mock<IMapper> _mockMapper;
    private UserService _userService;

    [Test]
    public async Task AddAsync_ShouldReturnNewUser()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com" };
        _mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync(user);

        // Act
        var result = await _userService.AddAsync(user);

        // Assert
        Assert.That(result, Is.EqualTo(user));
        _mockUserRepository.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, Email = "test@example.com" },
            new() { Id = 2, Email = "another@example.com" }
        };
        _mockUserRepository.Setup(repo => repo.GetAll()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetByEmailAsync("test@example.com");

        // Assert
        Assert.That(result.Email, Is.EqualTo("test@example.com"));
    }

    [Test]
    public void GetByEmailAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var users = new List<User>();
        _mockUserRepository.Setup(repo => repo.GetAll()).ReturnsAsync(users);

        // Act & Assert
        Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await _userService.GetByEmailAsync("nonexistent@example.com"));
    }

    [Test]
    public async Task MakeEducator_ShouldUpdateUserToEducator()
    {
        // Arrange
        var user = new User { Id = 1, IsEducator = false };
        var updatedUser = new User { Id = 1, IsEducator = true };
        var userProfileDto = new UserProfileDto { Id = 1, IsEducator = true };

        _mockUserRepository.Setup(repo => repo.GetByKey(1)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>())).ReturnsAsync(updatedUser);
        _mockMapper.Setup(mapper => mapper.Map<UserProfileDto>(updatedUser)).Returns(userProfileDto);

        // Act
        var result = await _userService.MakeEducator(1);

        // Assert
        Assert.That(result.IsEducator, Is.True);
        _mockUserRepository.Verify(repo => repo.Update(It.Is<User>(u => u.IsEducator)), Times.Once);
    }

    [Test]
    public async Task UpdateProfile_ShouldUpdateUserProfile()
    {
        // Arrange
        var userProfileDto = new UserProfileDto
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            ProfilePictureUrl = "http://example.com/pic.jpg"
        };
        var user = new User { Id = 1 };
        var updatedUser = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            ProfilePictureUrl = "http://example.com/pic.jpg"
        };

        _mockUserRepository.Setup(repo => repo.GetByKey(1)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>())).ReturnsAsync(updatedUser);
        _mockMapper.Setup(mapper => mapper.Map<UserProfileDto>(updatedUser)).Returns(userProfileDto);

        // Act
        var result = await _userService.UpdateProfile(userProfileDto);

        // Assert
        Assert.That(result, Is.EqualTo(userProfileDto));
        _mockUserRepository.Verify(repo => repo.Update(It.Is<User>(u =>
            u.FirstName == "John" &&
            u.LastName == "Doe" &&
            u.Email == "john@example.com" &&
            u.ProfilePictureUrl == "http://example.com/pic.jpg")), Times.Once);
    }

    [Test]
    public async Task UpdateProfileEntries_ShouldUpdateUserProfileEntries()
    {
        // Arrange
        var userProfileUpdateDto = new UserProfileUpdateDto
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com"
        };
        var user = new User { Id = 1 };
        var updatedUser = new User
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com"
        };
        var userProfileDto = new UserProfileDto
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com"
        };

        _mockUserRepository.Setup(repo => repo.GetByKey(1)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>())).ReturnsAsync(updatedUser);
        _mockMapper.Setup(mapper => mapper.Map<UserProfileDto>(updatedUser)).Returns(userProfileDto);

        // Act
        var result = await _userService.UpdateProfileEntries(userProfileUpdateDto);

        // Assert
        Assert.That(result, Is.EqualTo(userProfileDto));
        _mockUserRepository.Verify(repo => repo.Update(It.Is<User>(u =>
            u.FirstName == "Jane" &&
            u.LastName == "Doe" &&
            u.Email == "jane@example.com")), Times.Once);
    }
}