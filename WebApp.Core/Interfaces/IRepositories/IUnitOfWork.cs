namespace WebApp.Core.Interfaces.IRepositories;

public interface IUnitOfWork
{
    IReportRepository ReportRepository { get; }

    IMessageRepository MessageRepository { get; }

    ITopicRepository TopicRepository { get; }

    IUserRepository UserRepository { get; }

    Task SaveAsync();
}
