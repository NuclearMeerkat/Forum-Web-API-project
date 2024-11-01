namespace WebApp.DataAccess.Entities;

public class Report : BaseEntity
{
    public int MessageId { get; set; }

    public int UserId { get; set; }

    public string Reason { get; set; }

    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public Message Message { get; set; }

    public User User { get; set; }
}

public enum ReportStatus
{
    Pending,
    UnderReview,
    Resolved,
    Rejected,
}
