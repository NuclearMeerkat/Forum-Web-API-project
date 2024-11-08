using System.Diagnostics.CodeAnalysis;

namespace WebApp.Core.Models.TopicModels;

public class TopicQueryParametersModel
{
    public int Page { get; set; } = 1;         // Default to the first page
    public int Size { get; set; } = 10;        // Default page size

    public string Search { get; set; } = string.Empty;         // Optional search term for filtering
    public string SortBy { get; set; } = "Title"; // Default sorting by Title
    public bool Ascending { get; set; } = true;

    // Optional flag to retrieve all topics if true
    public bool RetrieveAll { get; set; } = false;
}
