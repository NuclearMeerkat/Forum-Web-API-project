using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Repositories;
using WebApp.Tests.Comparers;

namespace WebApp.Tests.Data;
internal class MessageLikeRepositoryTests
{
    [TestCase(1, 1)]
    [TestCase(2, 1)]
    public async Task MessageLikeRepositoryGetByIdAsyncReturnsSingleValue(int UserId, int MessageId)
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var messageLikeRepository = new MessageLikeRepository(context);

        var messageLike = await messageLikeRepository.GetByIdAsync(UserId, MessageId);

        var expected = ExpectedMessageLikes.FirstOrDefault(x => x.MessageId == MessageId && x.UserId == UserId);

        Assert.That(messageLike, Is.EqualTo(expected).Using(new MessageLikeEqualityComparer()), message: "GetByIdAsync method works incorrectly");
    }

    [Test]
    public async Task MessageLikeRepositoryGetAllAsyncReturnsAllValues()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var messageLikeRepository = new MessageLikeRepository(context);

        var messageLikes = await messageLikeRepository.GetAllAsync();

        Assert.That(messageLikes, Is.EqualTo(ExpectedMessageLikes).Using(new MessageLikeEqualityComparer()), message: "GetAllAsync method works incorrectly");
    }

    [Test]
    public async Task MessageLikeRepositoryAddAsyncAddsValueToDatabase()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var messageLikeRepository = new MessageLikeRepository(context);
        var messageLike = new MessageLike { UserId = 1, MessageId = 3 };

        await messageLikeRepository.AddAsync(messageLike);
        await context.SaveChangesAsync();

        Assert.That(context.Likes.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
    }

    [Test]
    public async Task MessageLikeRepositoryDeleteByIdAsyncDeletesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var messageLikeRepository = new MessageLikeRepository(context);

        await messageLikeRepository.DeleteByIdAsync(2, 1);
        await context.SaveChangesAsync();

        Assert.That(context.Likes.Count(), Is.EqualTo(2), message: "DeleteByIdAsync method works incorrectly");
    }

    private static IEnumerable<MessageLike> ExpectedMessageLikes =>
        new[]
        {
            new MessageLike { MessageId = 1, UserId = 2 },
            new MessageLike { MessageId = 1, UserId = 3 },
            new MessageLike { MessageId = 2, UserId = 1 },
        };
}
