using Microsoft.EntityFrameworkCore;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.DataAccess.Data;

namespace WebApp.DataAccess.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(ForumDbContext context)
        : base(context)
    {
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
}
