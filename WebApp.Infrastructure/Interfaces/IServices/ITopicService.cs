namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Models.TopicModels;

public interface ITopicService : ICrud<TopicSummaryModel, AdminTopicCreateModel, TopicUpdateModel, TopicQueryParametersModel, int>
{
    public Task RateTopic(int userId, int topicId, int stars);

    public Task RemoveRate(int userId, int topicId);

    public Task<bool> CheckTopicOwner(int topicId, int userId);

    public Task<TopicDialogModel> GetByIdWithDetailsAsync(params object[] keys);
}
