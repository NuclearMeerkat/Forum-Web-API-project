using System.Diagnostics.CodeAnalysis;
using WebApp.Core.Entities;

namespace WebApp.Tests.Comparers;

internal sealed class ReportEqualityComparer : IEqualityComparer<Report>
{
    public bool Equals([AllowNull] Report x, [AllowNull] Report y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.MessageId == y.MessageId
            && x.UserId == y.UserId
            && x.Reason == y.Reason
            && x.Status == y.Status;
    }

    public int GetHashCode([DisallowNull] Report obj)
    {
        return obj.GetHashCode();
    }
}
