using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;

namespace WebApp.DataAccess.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(ForumDbContext context)
        : base(context)
    {
    }

    public override async Task<Message> GetByIdAsync(params object[] keys)
    {
        return await this.context.Set<Message>()
            .Include(m => m.User)
            .Include(m => m.Replies)
            .FirstAsync(m => m.Id == (int)keys.ElementAt(0));
    }

    public override async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await this.context.Set<Message>()
            .Include(m => m.User)
            .Include(m => m.Replies)
            .ToListAsync();
    }

    public async Task<Message> GetByIdWithDetailsAsync(int id)
    {
        return await this.context.Messages
            .Include(m => m.Replies)
            .Include(m => m.Topic)
            .Include(m => m.User)
            .Include(m => m.Reports)
            .FirstAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Message>> GetAllWithDetailsAsync()
    {
        return await this.context.Messages
            .Include(m => m.Replies)
            .Include(m => m.Topic)
            .Include(m => m.User)
            .Include(m => m.Reports)
            .ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetByTopicId(int topicId)
    {
        return await this.context.Set<Message>()
            .Include(m => m.User)
            .Include(m => m.Replies)
            .Where(m => m.TopicId == topicId)
            .ToListAsync();
    }

    public override async Task DeleteByIdAsync(params object[] keys)
    {
        var entity = await this.context.Set<Message>().FindAsync(keys);
        if (entity != null)
        {
            var dependentReports = this.context.Reports.Where(ts => ts.UserId == (int)keys[0]);
            var dependentLikes = this.context.Likes.Where(ts => ts.UserId == (int)keys[0]);

            this.context.Reports.RemoveRange(dependentReports);
            this.context.Likes.RemoveRange(dependentLikes);

            _ = this.context.Set<Message>().Remove(entity);
            _ = await this.context.SaveChangesAsync();
        }
    }
}
