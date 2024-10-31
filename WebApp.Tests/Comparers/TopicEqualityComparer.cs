using System.Diagnostics.CodeAnalysis;
using WebApp.DataAccess.Entities;

namespace WebApp.Tests.Comparers;

internal sealed class TopicEqualityComparer : IEqualityComparer<Topic>
{
    public bool Equals([AllowNull] Topic x, [AllowNull] Topic y)
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
            && x.Title == y.Title
            && x.Description == y.Description
            && x.UserId == y.UserId;
    }

    public int GetHashCode([DisallowNull] Topic obj)
    {
        return obj.GetHashCode();
    }
}
