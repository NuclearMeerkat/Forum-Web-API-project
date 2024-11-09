using System.Diagnostics.CodeAnalysis;
using WebApp.Infrastructure.Entities;

namespace WebApp.Tests.Comparers;

internal sealed class UserEqualityComparer : IEqualityComparer<User>
{
    public bool Equals([AllowNull] User x, [AllowNull] User y)
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
            && x.Nickname == y.Nickname
            && x.Email == y.Email
            && x.ProfilePictureUrl == y.ProfilePictureUrl
            && x.Role == y.Role;
    }

    public int GetHashCode([DisallowNull] User obj)
    {
        return obj.GetHashCode();
    }
}
