using GameShelf.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameShelf.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
  private readonly ILogger<GlobalExceptionHandler> _logger;

  public GlobalExceptionHandler(
      ILogger<GlobalExceptionHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    if (exception is InvalidQueryParameterException)
    {
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status400BadRequest,
        Title = "Invalid query parameter.",
        Detail = exception.Message,
        Instance = httpContext.Request.Path
      };

      httpContext.Response.StatusCode =
          StatusCodes.Status400BadRequest;

      await httpContext.Response.WriteAsJsonAsync(
          problemDetails,
          cancellationToken);

      return true;
    }

    _logger.LogError(
        exception,
        "An unhandled exception occurred while processing the request.");

    var internalError = new ProblemDetails
    {
      Status = StatusCodes.Status500InternalServerError,
      Title = "An unexpected error occurred.",
      Instance = httpContext.Request.Path
    };

    httpContext.Response.StatusCode =
        StatusCodes.Status500InternalServerError;

    await httpContext.Response.WriteAsJsonAsync(
        internalError,
        cancellationToken);

    return true;
  }
}