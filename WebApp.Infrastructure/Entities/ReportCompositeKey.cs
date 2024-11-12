namespace WebApp.Infrastructure.Entities;

public class ReportCompositeKey
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is ReportCompositeKey otherKey)
        {
            return this.UserId == otherKey.UserId && this.MessageId == otherKey.MessageId;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.UserId, this.MessageId);
    }
}
