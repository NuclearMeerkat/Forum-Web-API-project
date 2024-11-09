﻿using System.Text.Json.Serialization;
using WebApp.Infrastructure.Enums;

namespace WebApp.Infrastructure.Models.ReportModels;

public class ReportUpdateModel
{
    public int UserId { get; set; }

    public int MessageId { get; set; }
    public string? Reason { get; set; }

    public ReportStatus? Status { get; set; }

    [JsonIgnore]
    public DateTime? ReviewedAt { get; set; }
}