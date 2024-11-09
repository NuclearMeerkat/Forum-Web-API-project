using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Models.ReportModels;

public class ReportCreateModel
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public string Reason { get; set; }


    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
