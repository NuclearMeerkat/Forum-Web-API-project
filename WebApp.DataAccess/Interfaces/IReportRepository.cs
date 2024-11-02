using WebApp.DataAccess.Entities;

namespace WebApp.DataAccess.Interfaces;

public interface IReportRepository
{
    public Task<Report> GetWithDetailsAsync(int userId, int messageId);

    public Task<IEnumerable<Report>> GetAllWithDetailsAsync();
}
