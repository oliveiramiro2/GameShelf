namespace GameShelf.Api.DTOs;

public class GameQueryParameters : PaginationParameters
{
  public string? Genre { get; set; }
  public string? Search { get; set; }

  public string? SortBy { get; set; }
  public bool Descending { get; set; }
}