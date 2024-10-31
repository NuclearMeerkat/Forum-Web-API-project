using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Repositories;
using WebApp.Tests.Comparers;

namespace WebApp.Tests.Data;
internal class MessageRepositoryTests
{
    [TestCase(1)]
    [TestCase(2)]
    public async Task MessageRepositoryGetByIdAsyncReturnsSingleValue(int id)
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var customerRepository = new MessageRepository(context);

        var customer = await customerRepository.GetByIdAsync(id);

        var expected = ExpectedMessages.FirstOrDefault(x => x.Id == id);

        Assert.That(customer, Is.EqualTo(expected).Using(new MessageEqualityComparer()), message: "GetByIdAsync method works incorrect");
    }

    [Test]
    public async Task CustomerRepositoryGetAllAsyncReturnsAllValues()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var customerRepository = new MessageRepository(context);

        var customers = await customerRepository.GetAllAsync();

        Assert.That(customers, Is.EqualTo(ExpectedMessages).Using(new MessageEqualityComparer()), message: "GetAllAsync method works incorrect");
    }

    [Test]
    public async Task CustomerRepositoryAddAsyncAddsValueToDatabase()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var customerRepository = new MessageRepository(context);
        var customer = new Message
        {
            Content = "Updated content",
            LikesCounter = 50,
            TopicId = 2,
            UserId = 2,
            ParentMessageId = null,
        };

        await customerRepository.AddAsync(customer);
        await context.SaveChangesAsync();

        Assert.That(context.Messages.Count(), Is.EqualTo(4), message: "AddAsync method works incorrect");
    }

    [Test]
    public async Task CustomerRepositoryDeleteByIdAsyncDeletesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var customerRepository = new MessageRepository(context);

        await customerRepository.DeleteByIdAsync(1);
        await context.SaveChangesAsync();

        Assert.That(context.Messages.Count(), Is.EqualTo(2), message: "DeleteByIdAsync works incorrect");
    }

    [Test]
    public async Task CustomerRepositoryUpdateUpdatesEntity()
    {
        using var context = new ForumDbContext(UnitTestDataHelper.GetUnitTestDbOptions());

        var customerRepository = new MessageRepository(context);
        var customer = new Message
        {
            Id = 1,
            Content = "Updated content",
            LikesCounter = 50,
            TopicId = 2,
            UserId = 2,
            ParentMessageId = null,
        };

        customerRepository.Update(customer);
        await context.SaveChangesAsync();

        Assert.That(customer, Is.EqualTo(new Message
        {
            Id = 1,
            Content = "Updated content",
            LikesCounter = 50,
            TopicId = 2,
            UserId = 2,
            ParentMessageId = null,
        }).Using(new MessageEqualityComparer()), message: "Update method works incorrect");
    }

    private static IEnumerable<Message> ExpectedMessages =>
        new[]
        {
                new Message { Id = 1, Content = "Hello everyone!", UserId = 1, TopicId = 1, LikesCounter = 2 },
                new Message { Id = 2, Content = "Nice to meet you all!", UserId = 2, TopicId = 1, LikesCounter = 1, ParentMessageId = 1 }, // Reply to Message 1
                new Message { Id = 3, Content = "What's new in tech?", UserId = 3, TopicId = 3, LikesCounter = 0 }
        };
}
