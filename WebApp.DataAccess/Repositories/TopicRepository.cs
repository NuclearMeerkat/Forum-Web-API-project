using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Interfaces;

namespace WebApp.DataAccess.Repositories;

public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<Topic> GetWithDetailsAsync(int id)
    {
        return await context.Topics
            .Include(t => t.Messages)
            .Include(t => t.User)
            .FirstAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Topic>> GetAllWithDetailsAsync()
    {
        return await context.Topics
            .Include(t => t.Messages)
            .Include(t => t.User)
            .ToListAsync();
    }
}
