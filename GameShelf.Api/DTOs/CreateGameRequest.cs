using System.ComponentModel.DataAnnotations;

namespace GameShelf.Api.DTOs;

public class CreateGameRequest
{
  [Required]
  public string Title { get; set; } = string.Empty;

  [Required]
  public string Genre { get; set; } = string.Empty;

  [Range(1950, 2026)]
  public int ReleaseYear { get; set; }
}