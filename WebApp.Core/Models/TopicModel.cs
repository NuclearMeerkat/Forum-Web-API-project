namespace WebApp.Core.Models;

public class TopicModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public double AverageStars { get; set; }

    public int EvaluationsNumber { get; set; }

    public double ActivityScore { get; set; }

    public string CreatorNickname { get; set; }

    public string CreatorEmail { get; set; }

    public string? CreatorProfilePictureUrl { get; set; }

    public ICollection<MessageModel> Messages { get; set; } = new List<MessageModel>();
}
