
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repositories;
using WebApp.Infrastructure.Entities;
using WebApp.Tests.Comparers;

namespace WebApp.Tests.Data;
internal class TopicRepositoryTests
{
    [TestCase(1)]
    [TestCase(2)]
    public async Task TopicRepositoryGetByIdAsyncReturnsSingleValue(int id)
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicRepository = new TopicRepository(context);

        var topic = await topicRepository.GetByIdAsync(id);

        var expected = ExpectedTopics.FirstOrDefault(x => x.Id == id);

        Assert.That(topic, Is.EqualTo(expected).Using(new TopicEqualityComparer()), message: "GetByIdAsync method works incorrect");
    }

    [Test]
    public async Task TopicRepositoryGetAllAsyncReturnsAllValues()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicRepository = new TopicRepository(context);

        var topics = await topicRepository.GetAllAsync();

        Assert.That(topics, Is.EqualTo(ExpectedTopics).Using(new TopicEqualityComparer()), message: "GetAllAsync method works incorrect");
    }

    [Test]
    public async Task TopicRepositoryAddAsyncAddsValueToDatabase()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicRepository = new TopicRepository(context);
        var topic = new Topic { Id = 4, Title = "New Topic", Description = "Discussion about new topic" };

        await topicRepository.AddAsync(topic);
        await context.SaveChangesAsync();

        Assert.That(context.Topics.Count(), Is.EqualTo(4), message: "AddAsync method works incorrect");
    }

    [Test]
    public async Task TopicRepositoryDeleteByIdAsyncDeletesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicRepository = new TopicRepository(context);

        await topicRepository.DeleteByIdAsync(1);
        await context.SaveChangesAsync();

        Assert.That(context.Topics.Count(), Is.EqualTo(2), message: "DeleteByIdAsync works incorrect");
    }

    [Test]
    public async Task TopicRepositoryUpdateUpdatesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var topicRepository = new TopicRepository(context);
        var topic = new Topic
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            UserId = 1
        };

        topicRepository.Update(topic);
        await context.SaveChangesAsync();

        Assert.That(topic, Is.EqualTo(new Topic
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Description",
            UserId = 1
        }).Using(new TopicEqualityComparer()), message: "Update method works incorrect");
    }

    private static IEnumerable<Topic> ExpectedTopics =>
        new[]
        {
            new Topic { Id = 1, Title = "Welcome to the Forum", Description = "Introduce yourself here!", UserId = 1 },
            new Topic { Id = 2, Title = "General Discussion", Description = "Talk about anything here!", UserId = 2 },
            new Topic { Id = 3, Title = "Tech Talk", Description = "Discuss the latest in technology!", UserId = 3 }
        };
}
