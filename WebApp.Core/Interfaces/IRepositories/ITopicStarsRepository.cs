using WebApp.Core.Entities;

namespace WebApp.Core.Interfaces.IRepositories;
public interface ITopicStarsRepository : IRepository<TopicStars>
{
    public Task<double> GetAverageStarsForTopicAsync(int topicId);

    public Task<int> GetEvaluationsNumberForTopicAsync(int topicId);
}
