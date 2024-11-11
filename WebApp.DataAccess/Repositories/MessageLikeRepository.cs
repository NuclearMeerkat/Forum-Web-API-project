using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;

namespace WebApp.DataAccess.Repositories;

public class MessageLikeRepository : GenericRepository<MessageLike>, IMessageLikeRepository
{
    public MessageLikeRepository(ForumDbContext context)
        : base(context)
    {
    }
}
