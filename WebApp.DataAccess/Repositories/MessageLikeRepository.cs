using WebApp.Core.Entities;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.DataAccess.Data;

namespace WebApp.DataAccess.Repositories;

public class MessageLikeRepository : GenericRepository<MessageLike>, IMessageLikeRepository
{
    public MessageLikeRepository(ForumDbContext context)
        : base(context)
    {
    }
}
