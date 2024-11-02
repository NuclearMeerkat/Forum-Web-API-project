using WebApp.Core.Enums;

namespace WebApp.Core.Models;

public class ReportSummaryModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Reason { get; set; }

    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
