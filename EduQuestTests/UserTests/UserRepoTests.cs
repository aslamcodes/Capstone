using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.UserTests;

[TestFixture]
public class UserRepoTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _userRepo = new UserRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private UserRepo _userRepo;

    private User CreateTestUser(string email)
    {
        return new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Password = new byte[] { 1, 2, 3, 4 },
            PasswordHashKey = new byte[] { 5, 6, 7, 8 },
            ProfilePictureUrl = "https://example.com/profile.jpg",
            Status = UserStatusEnum.ACTIVE,
            IsEducator = false,
            IsAdmin = false
        };
    }

    [Test]
    public async Task Add_ShouldAddUserToDatabase()
    {
        // Arrange
        var user = CreateTestUser("john.doe@example.com");

        // Act
        var result = await _userRepo.Add(user);

        // Assert
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.Users.Count(), Is.EqualTo(1));
        Assert.That(_context.Users.First().Email, Is.EqualTo("john.doe@example.com"));
    }

    [Test]
    public async Task GetByKey_ShouldReturnCorrectUser()
    {
        // Arrange
        var user = CreateTestUser("jane.smith@example.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepo.GetByKey(user.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(user.Id));
        Assert.That(result.Email, Is.EqualTo("jane.smith@example.com"));
    }

    [Test]
    public async Task Update_ShouldUpdateUserInDatabase()
    {
        // Arrange
        var user = CreateTestUser("bob.johnson@example.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        user.Email = "bob.updated@example.com";
        var result = await _userRepo.Update(user);

        // Assert
        Assert.That(result.Email, Is.EqualTo("bob.updated@example.com"));
        var updatedUser = await _context.Users.FindAsync(user.Id);
        Assert.That(updatedUser.Email, Is.EqualTo("bob.updated@example.com"));
    }

    [Test]
    public async Task Delete_ShouldRemoveUserFromDatabase()
    {
        // Arrange
        var user = CreateTestUser("alice.brown@example.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        await _userRepo.Delete(user.Id);

        // Assert
        Assert.That(_context.Users.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new[]
        {
            CreateTestUser("user1@example.com"),
            CreateTestUser("user2@example.com"),
            CreateTestUser("user3@example.com")
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepo.GetAll();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(3));
        Assert.That(result.Select(u => u.Email),
            Is.EquivalentTo(new[] { "user1@example.com", "user2@example.com", "user3@example.com" }));
    }
}