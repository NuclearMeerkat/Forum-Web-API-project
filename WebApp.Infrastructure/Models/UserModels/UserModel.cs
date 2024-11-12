namespace WebApp.Infrastructure.Models.UserModels;

using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.ReportModels;
using WebApp.Infrastructure.Models.TopicModels;

public class UserModel
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public ICollection<TopicSummaryModel> ParticipatedTopics { get; set; } = new List<TopicSummaryModel>();

    public ICollection<TopicSummaryModel> OwnedTopics { get; set; } = new List<TopicSummaryModel>();

    public ICollection<MessageBriefModel> Messages { get; set; } = new List<MessageBriefModel>();

    public ICollection<ReportSummaryModel> Reports { get; set; } = new List<ReportSummaryModel>();
}
