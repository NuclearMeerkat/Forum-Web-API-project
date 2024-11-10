﻿using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Models.TopicModels;

public class TopicCreateModel
{
    [JsonIgnore]
    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
