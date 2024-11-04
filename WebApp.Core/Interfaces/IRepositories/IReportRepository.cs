using WebApp.Core.Entities;

namespace WebApp.Core.Interfaces.IRepositories;

public interface IReportRepository : IRepository<Report>
{
    public Task<Report> GetWithDetailsAsync(int userId, int messageId);

    public Task<IEnumerable<Report>> GetAllWithDetailsAsync();
}
