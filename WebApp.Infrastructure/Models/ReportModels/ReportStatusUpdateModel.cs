namespace WebApp.Infrastructure.Models.ReportModels;

using WebApp.Infrastructure.Enums;

public class ReportStatusUpdateModel
{
    public int MessageId { get; set; }

    public ReportStatus? Status { get; set; }
}
