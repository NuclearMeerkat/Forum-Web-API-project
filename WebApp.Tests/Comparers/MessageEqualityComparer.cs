using System.Diagnostics.CodeAnalysis;
using WebApp.Core.Entities;

namespace WebApp.Tests.Comparers;

internal sealed class MessageEqualityComparer : IEqualityComparer<Message>
{
    public bool Equals([AllowNull] Message x, [AllowNull] Message y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Id == y.Id
            && x.Content == y.Content
            && x.IsEdited == y.IsEdited
            && x.LikesCounter == y.LikesCounter
            && x.TopicId == y.TopicId
            && x.UserId == y.UserId
            && x.ParentMessageId == y.ParentMessageId;
    }

    public int GetHashCode([DisallowNull] Message obj)
    {
        return obj.GetHashCode();
    }
}
