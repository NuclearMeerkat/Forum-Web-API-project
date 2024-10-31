using WebApp.DataAccess.Interfaces;
using WebApp.DataAccess.Repositories;

namespace WebApp.DataAccess.Data;

public class UnitOFWork : IUnitOfWork
{
    private readonly ForumDbContext context;

    public UnitOFWork(ForumDbContext context)
    {
        this.context = context;
        this.MessageRepository = new MessageRepository(context);
        this.ReportRepository = new ReportRepository(context);
        this.TopicRepository = new TopicRepository(context);
        this.UserRepository = new UserRepository(context);
    }

    public IMessageRepository MessageRepository { get; }

    public IReportRepository ReportRepository { get; }

    public ITopicRepository TopicRepository { get; }

    public IUserRepository UserRepository { get; }

    public async Task SaveAsync()
    {
        _ = await this.context.SaveChangesAsync();
    }
}
