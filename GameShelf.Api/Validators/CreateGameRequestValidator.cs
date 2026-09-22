using GameShelf.Api.DTOs;

namespace GameShelf.Api.Validators;

public class CreateGameRequestValidator
{
  public List<string> Validate(CreateGameRequest request)
  {
    var errors = new List<string>();

    if (string.IsNullOrWhiteSpace(request.Title))
    {
      errors.Add("Title is required.");
    }

    if (string.IsNullOrWhiteSpace(request.Genre))
    {
      errors.Add("Genre is required.");
    }

    if (request.ReleaseYear < 1950 || request.ReleaseYear > DateTime.Today.Year)
    {
      errors.Add("ReleaseYear must be greater than or equal to 1950 and less than the current year.");
    }

    return errors;
  }
}