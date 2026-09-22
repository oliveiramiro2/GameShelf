using System.ComponentModel.DataAnnotations;

namespace GameShelf.Api.Validators;

public class ReleaseYearAttribute : ValidationAttribute
{
  public ReleaseYearAttribute()
  {
    ErrorMessage = "ReleaseYear must be between 1950 and the current year.";
  }

  public override bool IsValid(object? value)
  {
    if (value is not int year)
    {
      return false;
    }

    return year >= 1950 && year <= DateTime.Today.Year;
  }
}