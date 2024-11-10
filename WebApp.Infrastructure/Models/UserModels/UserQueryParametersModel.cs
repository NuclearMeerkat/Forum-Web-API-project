namespace WebApp.Infrastructure.Models;

public class UserQueryParametersModel
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;

    public string Search { get; set; } = string.Empty;
    public string SortBy { get; set; } = "Nickname";

    public bool Ascending { get; set; } = true;
}
