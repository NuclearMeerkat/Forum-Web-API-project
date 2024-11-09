using WebApp.Infrastructure.Entities;

namespace WebApp.Infrastructure.Interfaces.IRepositories;
public interface ITopicStarsRepository : IRepository<TopicStars>
{
    public Task<double> GetAverageStarsForTopicAsync(int topicId);

    public Task<int> GetEvaluationsNumberForTopicAsync(int topicId);
}
