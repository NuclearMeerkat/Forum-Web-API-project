using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;

namespace WebApp.DataAccess.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Report>> GetReportsForTopicAsync(int topicId)
    {
        return await this.context.Reports
            .Include(r => r.User)
            .Include(r => r.Message)
                .ThenInclude(m => m.User)
            .Where(r => r.Message.TopicId == topicId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetRangeAsync(int skip, int take)
    {
        return await this.context.Reports
            .Include(r => r.User)
            .Include(r => r.Message)
            .ThenInclude(m => m.User)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
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
