namespace GameShelf.Api.Exceptions;

public class InvalidQueryParameterException : Exception
{
  public InvalidQueryParameterException(string message)
      : base(message)
  {
  }
}