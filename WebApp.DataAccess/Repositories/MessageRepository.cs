using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Interfaces;

namespace WebApp.DataAccess.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<Message> GetWithDetailsAsync(int id)
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
}
