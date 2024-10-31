using System.ComponentModel.DataAnnotations;

namespace WebApp.DataAccess.Entities;

public class TopicStars : BaseEntity
{
    [Range(0, 5, ErrorMessage = "Stars must be between 0 and 5.")]
    public int Stars { get; set; }

    public int UserId { get; set; }

    public int TopicId { get; set; }

    public User User { get; set; }

    public Topic Topic { get; set; }
}
