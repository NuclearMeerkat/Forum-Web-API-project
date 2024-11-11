using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;

namespace WebApp.DataAccess.Repositories;

public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(ForumDbContext context)
        : base(context)
    {
    }

    public override async Task<IEnumerable<Topic>> GetAllAsync()
    {
        return await this.context.Topics
            .Include(t => t.Stars)
            .Include(t => t.Messages)
            .AsQueryable()
            .Select(t => new Topic
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId,
                User = t.User,
                Messages = t.Messages,
                Stars = t.Stars,
                AverageStars = t.Stars.Count != 0 ? t.Stars.Average(s => (double)s.StarCount) : 0,
                EvaluationsNumber = t.Stars.Count,
            })
            .ToListAsync();
    }

    public override async Task<Topic> GetByIdAsync(params object[] keys)
    {
        return await this.context.Topics
            .Include(t => t.Stars)
            .Include(t => t.Messages)
            .AsQueryable()
            .Where(t => t.Id == (int)keys[0])
            .Select(t => new Topic
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId,
                User = t.User,
                Messages = t.Messages,
                Stars = t.Stars,
                AverageStars = t.Stars.Count != 0 ? t.Stars.Average(s => (double)s.StarCount) : 0,
                EvaluationsNumber = t.Stars.Count,
            })
            .FirstAsync();
    }

    public async Task<Topic> GetWithDetailsAsync(int id)
    {
        return await this.context.Topics
            .Include(t => t.Stars)
            .Include(t => t.Messages)
            .Include(t => t.User)
            .AsQueryable()
            .Where(t => t.Id == id)
            .Select(t => new Topic
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId,
                User = t.User,
                Messages = t.Messages,
                Stars = t.Stars,
                AverageStars = t.Stars.Count != 0 ? t.Stars.Average(s => (double)s.StarCount) : 0,
                EvaluationsNumber = t.Stars.Count,
            })
            .FirstAsync();
    }

    public async Task<IEnumerable<Topic>> GetAllWithDetailsAsync()
    {
        return await this.context.Topics
            .Include(t => t.Stars)
            .Include(t => t.Messages)
            .Include(t => t.User)
            .AsQueryable()
            .Select(t => new Topic
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId,
                User = t.User,
                Messages = t.Messages,
                Stars = t.Stars,
                AverageStars = t.Stars.Count != 0 ? t.Stars.Average(s => (double)s.StarCount) : 0,
                EvaluationsNumber = t.Stars.Count,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<Topic>> GetRangeAsync(int skip, int take)
    {
        return await this.context.Topics
            .Include(t => t.Stars)
            .Include(t => t.Messages)
            .AsQueryable()
            .Select(t => new Topic
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId,
                User = t.User,
                Messages = t.Messages,
                Stars = t.Stars,
                AverageStars = t.Stars.Count != 0 ? t.Stars.Average(s => (double)s.StarCount) : 0,
                EvaluationsNumber = t.Stars.Count,
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
