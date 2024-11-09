namespace WebApp.Infrastructure.Models.ReportModels;

public class ReportQueryParametersModel
{
    public int Page { get; set; } = 1;

    public int Size { get; set; } = 10;

    public string Search { get; set; } = string.Empty;

    public string SortBy { get; set; } = "Reason";

    public bool Ascending { get; set; } = true;

    public bool RetrieveAll { get; set; } = false;
}
