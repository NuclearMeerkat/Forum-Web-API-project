using WebApp.DataAccess.Data;
using WebApp.DataAccess.Entities;
using WebApp.DataAccess.Interfaces;

namespace WebApp.DataAccess.Repositories;

public class MessageLikeRepository : GenericRepository<MessageLike>, IMessageLikeRepository
{
    public MessageLikeRepository(ForumDbContext context)
        : base(context)
    {
    }
}
