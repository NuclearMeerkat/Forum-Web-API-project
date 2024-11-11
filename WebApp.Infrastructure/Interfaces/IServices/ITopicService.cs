namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Models.TopicModels;

public interface ITopicService
{
    Task<IEnumerable<TopicSummaryModel>> GetAllAsync(TopicQueryParametersModel? queryParameters = default);

    Task<TopicSummaryModel> GetByIdAsync(params object[] keys);

    Task<int> AddAsync(AdminTopicCreateModel model);

    Task UpdateAsync(TopicUpdateModel model);

    Task DeleteAsync(int modelId);

    public Task RateTopic(int userId, int topicId, int stars);

    public Task RemoveRate(int userId, int topicId);

    public Task<bool> CheckTopicOwner(int topicId, int userId);

    public Task<TopicDialogModel> GetByIdWithDetailsAsync(params object[] keys);
}
