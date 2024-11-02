namespace WebApp.DataAccess.Interfaces;
public interface ITopicStarsRepository
{
    public Task<double> GetAverageStarsForTopicAsync(int topicId);
}
