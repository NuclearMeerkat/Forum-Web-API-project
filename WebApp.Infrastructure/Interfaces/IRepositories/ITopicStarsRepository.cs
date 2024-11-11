namespace WebApp.Infrastructure.Interfaces.IRepositories;

using WebApp.Infrastructure.Entities;

public interface ITopicStarsRepository : IRepository<TopicStars>
{
    public Task<double> GetAverageStarsForTopicAsync(int topicId);

    public Task<int> GetEvaluationsNumberForTopicAsync(int topicId);
}
