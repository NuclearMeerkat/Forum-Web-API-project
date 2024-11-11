namespace WebApp.Infrastructure.Interfaces.IRepositories;

using WebApp.Infrastructure.Entities;

public interface IReportRepository : IRepository<Report>
{
    public Task<Report> GetWithDetailsAsync(int userId, int messageId);

    public Task<IEnumerable<Report>> GetAllWithDetailsAsync();

    public Task<IEnumerable<Report>> GetReportsForTopicAsync(int topicId);

    Task<IEnumerable<Report>> GetRangeAsync(int skip, int take);
}
