using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.DataAccess.Data;

namespace WebApp.DataAccess.Repositories;

public class MessageLikeRepository : GenericRepository<MessageLike>, IMessageLikeRepository
{
    public MessageLikeRepository(ForumDbContext context)
        : base(context)
    {
    }
}
