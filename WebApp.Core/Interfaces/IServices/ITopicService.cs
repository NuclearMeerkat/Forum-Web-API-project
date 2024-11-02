using WebApp.Core.Models;

namespace WebApp.Core.Interfaces.IServices;
public interface ITopicService : ICrud<TopicModel>
{
    public Task RateTopic(int userId, int topicId, int stars);

    public Task GetAverageRating(int topicId);
}
