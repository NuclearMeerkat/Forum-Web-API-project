using WebApp.Infrastructure.Models;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.Infrastructure.Interfaces.IServices;
public interface ITopicService : ICrud<TopicSummaryModel, TopicCreateModel, TopicUpdateModel, TopicQueryParametersModel, int>
{
    public Task RateTopic(int userId, int topicId, int stars);

    public Task RemoveRate(int userId, int topicId);
}
