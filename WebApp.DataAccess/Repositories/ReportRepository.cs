using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Interfaces;

namespace WebApp.DataAccess.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<Report> GetWithDetailsAsync(int userId, int messageId)
    {
        return await this.context.Reports
            .Include(r => r.User)
            .Include(r => r.Message)
            .ThenInclude(m => m.Topic)
            .FirstAsync(r => r.UserId == userId && r.MessageId == messageId);
    }

    public async Task<IEnumerable<Report>> GetAllWithDetailsAsync()
    {
        return await this.context.Reports
            .Include(r => r.User)
            .Include(r => r.Message)
            .ThenInclude(m => m.Topic)
            .ToListAsync();
    }
}
