namespace WebApp.Infrastructure.Models.ReportModels;

using System.Text.Json.Serialization;
public class ReportCreateModel
{
    [JsonIgnore]
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public string Reason { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
