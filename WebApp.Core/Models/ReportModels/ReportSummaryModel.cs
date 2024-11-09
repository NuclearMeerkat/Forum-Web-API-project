using WebApp.Core.Enums;

namespace WebApp.Core.Models.ReportModels;

public class ReportSummaryModel
{
    public string Reporter { get; set; }
    public string ReportedUser { get; set; }

    public int MessageId { get; set; }

    public string Message { get; set; }
    public string Reason { get; set; }

    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }
}
