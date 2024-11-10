using WebApp.Infrastructure.Models;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.Infrastructure.Interfaces.IServices;
public interface ITopicService : ICrud<TopicSummaryModel, AdminTopicCreateModel, TopicUpdateModel, TopicQueryParametersModel, int>
{
    public Task RateTopic(int userId, int topicId, int stars);

    public Task RemoveRate(int userId, int topicId);

    public Task<bool> CheckTopicOwner(int topicId, int userId);

}
