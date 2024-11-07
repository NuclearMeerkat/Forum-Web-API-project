using WebApp.Core.Models;
using WebApp.Core.Models.TopicModels;

namespace WebApp.Core.Interfaces.IServices;
public interface ITopicService : ICrud<TopicSummaryModel, TopicCreateModel, TopicUpdateModel, TopicQueryParametersModel, int>
{
    public Task RateTopic(int userId, int topicId, int stars);

    public Task RemoveRate(int userId, int topicId);
}
