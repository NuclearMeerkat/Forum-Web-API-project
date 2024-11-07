using WebApp.Core.Models;

namespace WebApp.Core.Interfaces.IServices;
public interface ITopicService : ICrud<TopicModel, TopicCreateModel, int>
{
    public Task RateTopic(int userId, int topicId, int stars);

    public Task RemoveRate(int userId, int topicId);

    Task<IEnumerable<TopicSummaryModel>> GetTopicsAsync(TopicQueryParametersModel parameters);
}
