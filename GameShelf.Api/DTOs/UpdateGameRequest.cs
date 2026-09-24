using System.ComponentModel.DataAnnotations;
using GameShelf.Api.Validators;

namespace GameShelf.Api.DTOs;

public class UpdateGameRequest
{
  [Required]
  public string Title { get; set; } = string.Empty;

  [Required]
  public string Genre { get; set; } = string.Empty;

  [ReleaseYear]
  public int ReleaseYear { get; set; }
}