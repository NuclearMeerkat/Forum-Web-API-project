using System.Diagnostics.CodeAnalysis;
using WebApp.DataAccess.Entities;

namespace WebApp.Tests.Comparers;

internal sealed class TopicStarsEqualityComparer : IEqualityComparer<TopicStars>
{
    public bool Equals([AllowNull] TopicStars x, [AllowNull] TopicStars y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.StarCount == y.StarCount
            && x.UserId == y.UserId
            && x.TopicId == y.TopicId;
    }

    public int GetHashCode([DisallowNull] TopicStars obj)
    {
        return obj.GetHashCode();
    }
}
