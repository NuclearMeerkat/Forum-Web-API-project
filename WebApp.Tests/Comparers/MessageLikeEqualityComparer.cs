using System.Diagnostics.CodeAnalysis;
using WebApp.Core.Entities;

namespace WebApp.Tests.Comparers;

internal sealed class MessageLikeEqualityComparer : IEqualityComparer<MessageLike>
{
    public bool Equals([AllowNull] MessageLike x, [AllowNull] MessageLike y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.UserId == y.UserId
            && x.MessageId == y.MessageId;
    }

    public int GetHashCode([DisallowNull] MessageLike obj)
    {
        return obj.GetHashCode();
    }
}
