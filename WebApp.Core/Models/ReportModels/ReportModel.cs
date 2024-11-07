using WebApp.Core.Enums;
using WebApp.Core.Models.MessageModels;
using WebApp.Core.Models.UserModels;

namespace WebApp.Core.Models.ReportModels;

public class ReportModel
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public string Reason { get; set; }

    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public MessageModel Message { get; set; }

    public UserModel User { get; set; }
}
