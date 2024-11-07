namespace WebApp.Core.Models.ReportModels;

public class ReportUpdateModel
{
    public int Id { get; set; } // Required for identifying the report to update
    public string Reason { get; set; } // Optional, used to update the report reason
}
