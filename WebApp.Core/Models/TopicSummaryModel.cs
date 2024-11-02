namespace WebApp.Core.Models;

public class TopicSummaryModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public double AverageStars { get; set; }

    public double ActivityScore { get; set; }

    public string CreatorNickname { get; set; }

    public DateTime CreatedAt { get; set; }
}
