using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.ReportModels;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.Infrastructure.Models.UserModels;

public class UserModel
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public ICollection<TopicModel> ParticipatedTopics { get; set; } = new List<TopicModel>();

    public ICollection<TopicModel> OwnedTopics { get; set; } = new List<TopicModel>();

    public ICollection<MessageModel> Messages { get; set; } = new List<MessageModel>();

    public ICollection<ReportModel> Reports { get; set; } = new List<ReportModel>();
}
