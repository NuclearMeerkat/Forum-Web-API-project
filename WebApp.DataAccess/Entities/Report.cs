namespace WebApp.DataAccess.Entities;

public class Report : BaseEntity
{
    public int MessageId { get; set; }

    public int UserId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public Message Message { get; set; }

    public User User { get; set; }
}
