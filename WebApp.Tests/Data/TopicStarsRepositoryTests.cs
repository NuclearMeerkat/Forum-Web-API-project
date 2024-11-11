
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repositories;
using WebApp.Infrastructure.Entities;
using WebApp.Tests.Comparers;

namespace WebApp.Tests.Data;
internal sealed class TopicStarsRepositoryTests
{
    [TestCase(1, 1)]
    [TestCase(2, 1)]
    public async Task TopicStarsRepositoryGetByIdAsyncReturnsSingleValue(int UserId, int TopicId)
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicStarsRepository = new TopicStarsRepository(context);

        var topicStar = await topicStarsRepository.GetByIdAsync(UserId, TopicId);

        var expected = ExpectedTopicStars.FirstOrDefault(x => x.TopicId == TopicId && x.UserId == UserId);

        Assert.That(topicStar, Is.EqualTo(expected).Using(new TopicStarsEqualityComparer()), message: "GetByIdAsync method works incorrectly");
    }

    [Test]
    public async Task TopicStarsRepositoryGetAllAsyncReturnsAllValues()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicStarsRepository = new TopicStarsRepository(context);

        var topicStars = await topicStarsRepository.GetAllAsync();

        Assert.That(topicStars, Is.EqualTo(ExpectedTopicStars).Using(new TopicStarsEqualityComparer()), message: "GetAllAsync method works incorrectly");
    }

    [Test]
    public async Task TopicStarsRepositoryAddAsyncAddsValueToDatabase()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicStarsRepository = new TopicStarsRepository(context);
        var topicStar = new TopicStars { UserId = 1, TopicId = 3, StarCount = 4 };

        await topicStarsRepository.AddAsync(topicStar);
        await context.SaveChangesAsync();

        Assert.That(context.TopicStars.Count(), Is.EqualTo(4), message: "AddAsync method works incorrectly");
    }

    [Test]
    public async Task TopicStarsRepositoryDeleteByIdAsyncDeletesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicStarsRepository = new TopicStarsRepository(context);

        await topicStarsRepository.DeleteByIdAsync(2, 2);
        await context.SaveChangesAsync();

        Assert.That(context.TopicStars.Count(), Is.EqualTo(2), message: "DeleteByIdAsync method works incorrectly");
    }

    private static IEnumerable<TopicStars> ExpectedTopicStars =>
        new[]
        {
            new TopicStars { StarCount = 4, UserId = 1, TopicId = 1 },
            new TopicStars { StarCount = 5, UserId = 2, TopicId = 2 },
            new TopicStars { StarCount = 3, UserId = 3, TopicId = 3 }
        };
}
