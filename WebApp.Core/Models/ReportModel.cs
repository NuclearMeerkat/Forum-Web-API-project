using WebApp.Core.Enums;

namespace WebApp.Core.Models;

public class ReportModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MessageId { get; set; }

    public string Reason { get; set; }

    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public MessageModel Message { get; set; }

    public UserModel User { get; set; }
}
