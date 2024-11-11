namespace WebApp.Infrastructure.Models.TopicModels;

public class TopicQueryParametersModel
{
    public int Page { get; set; } = 1;

    public int Size { get; set; } = 10;

    public string Search { get; set; } = string.Empty;

    public string SortBy { get; set; } = "Title";

    public bool Ascending { get; set; } = true;

    public bool RetrieveAll { get; set; } = false;
}
