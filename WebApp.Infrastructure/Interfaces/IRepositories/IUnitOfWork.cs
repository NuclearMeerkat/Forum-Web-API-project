namespace WebApp.Infrastructure.Interfaces.IRepositories;

public interface IUnitOfWork
{
    IReportRepository ReportRepository { get; }

    IMessageRepository MessageRepository { get; }

    ITopicRepository TopicRepository { get; }

    IUserRepository UserRepository { get; }

    IMessageLikeRepository MessageLikeRepository { get; }

    ITopicStarsRepository TopicStarsRepository { get; }

    Task SaveAsync();
}
