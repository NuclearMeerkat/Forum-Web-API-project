﻿using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Models.MessageModels;

public class MessageCreateModel
{
    public int UserId { get; set; }

    [JsonIgnore]
    public int TopicId { get; set; }

    public string Content { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ParentMessageId { get; set; }
}