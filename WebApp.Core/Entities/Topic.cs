using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Core.Entities;

public class Topic : BaseEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    [NotMapped]
    public double AverageStars { get; set; }

    [NotMapped]
    public int EvaluationsNumber { get; set; }

    [NotMapped]
    public double ActivityScore { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<TopicStars> Stars { get; set; } = new List<TopicStars>();
}
