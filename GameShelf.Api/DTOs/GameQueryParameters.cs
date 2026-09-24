namespace GameShelf.Api.DTOs;

public class GameQueryParameters
{
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 20;

  public string? Genre { get; set; }
  public string? Search { get; set; }

  public string? SortBy { get; set; }
  public bool Descending { get; set; }
}