namespace WebApp.Core.Models.ReportModels;

public class ReportCreateModel
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public string Reason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
