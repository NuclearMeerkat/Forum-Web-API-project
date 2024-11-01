using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Repositories;
using WebApp.Tests.Comparers;

namespace WebApp.Tests.Data;
internal class UserRepositoryTests
{
    [TestCase(1)]
    [TestCase(2)]
    public async Task UserRepositoryGetByIdAsyncReturnsSingleValue(int id)
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var userRepository = new UserRepository(context);

        var user = await userRepository.GetByIdAsync(id);

        var expected = ExpectedUsers.FirstOrDefault(x => x.Id == id);

        Assert.That(user, Is.EqualTo(expected).Using(new UserEqualityComparer()), message: "GetByIdAsync method works incorrect");
    }

    [Test]
    public async Task UserRepositoryGetAllAsyncReturnsAllValues()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var userRepository = new UserRepository(context);

        var users = await userRepository.GetAllAsync();

        Assert.That(users, Is.EqualTo(ExpectedUsers).Using(new UserEqualityComparer()), message: "GetAllAsync method works incorrect");
    }

    [Test]
    public async Task UserRepositoryAddAsyncAddsValueToDatabase()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var userRepository = new UserRepository(context);
        var user = new User { Id = 4, Nickname = "NewUser", Email = "newuser@example.com", Role = UserRole.User, ProfilePictureUrl = "URL" };

        await userRepository.AddAsync(user);
        await context.SaveChangesAsync();

        Assert.That(context.Users.Count(), Is.EqualTo(4), message: "AddAsync method works incorrect");
    }

    [Test]
    public async Task UserRepositoryDeleteByIdAsyncDeletesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var userRepository = new UserRepository(context);

        await userRepository.DeleteByIdAsync(1);
        await context.SaveChangesAsync();

        Assert.That(context.Users.Count(), Is.EqualTo(2), message: "DeleteByIdAsync works incorrect");
    }

    [Test]
    public async Task UserRepositoryUpdateUpdatesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var userRepository = new UserRepository(context);
        var user = new User
        {
            Id = 1,
            Nickname = "UpdatedName",
            Email = "updated@example.com",
            Role = UserRole.User,
            ProfilePictureUrl = "123",
        };

        userRepository.Update(user);
        await context.SaveChangesAsync();

        Assert.That(user, Is.EqualTo(new User
        {
            Id = 1,
            Nickname = "UpdatedName",
            Email = "updated@example.com",
            Role = UserRole.User,
            ProfilePictureUrl = "123",
        }).Using(new UserEqualityComparer()), message: "Update method works incorrect");
    }

    private static IEnumerable<User> ExpectedUsers =>
        new[]
        {
            new User { Id = 1, Nickname = "JaneDoe", Email = "jane.doe@example.com", ProfilePictureUrl = "/images/jane.jpg", Role = UserRole.User },
            new User { Id = 2, Nickname = "JohnSmith", Email = "john.smith@example.com", ProfilePictureUrl = "/images/john.jpg", Role = UserRole.Moderator },
            new User { Id = 3, Nickname = "AliceBlue", Email = "alice.blue@example.com", ProfilePictureUrl = "/images/alice.jpg", Role = UserRole.Admin }
        };
}
