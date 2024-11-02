namespace WebApp.Core.Interfaces.IRepositories;
public interface ITopicStarsRepository
{
    public Task<double> GetAverageStarsForTopicAsync(int topicId);
}
