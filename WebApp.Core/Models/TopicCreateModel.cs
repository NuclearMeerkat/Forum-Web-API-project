using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Core.Models;

public class TopicCreateModel
{
    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    [NotMapped]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
