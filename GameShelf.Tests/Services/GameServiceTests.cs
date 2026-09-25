using GameShelf.Api.Data;
using GameShelf.Api.Models;
using GameShelf.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace GameShelf.Tests.Services;

public class GameServiceTests
{
  private static GameService CreateService(GameShelfDbContext context)
  {
    var logger = NullLogger<GameService>.Instance;

    return new GameService(context, logger);
  }

  private static GameShelfDbContext CreateContext()
  {
    var options = new DbContextOptionsBuilder<GameShelfDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    return new GameShelfDbContext(options);
  }

  [Fact]
  public async Task GetById_ShouldReturnGame_WhenGameExists()
  {
    // Arrange
    await using var context = CreateContext();

    context.Games.Add(new Game
    {
      Id = 1,
      Title = "Hollow Knight",
      Genre = "Metroidvania",
      ReleaseYear = 2017
    });

    await context.SaveChangesAsync();

    var service = CreateService(context);

    // Act
    var result = await service.GetById(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Hollow Knight", result.Title);
  }

  [Fact]
  public async Task GetById_ShouldReturnNull_WhenGameDoesNotExist()
  {
    // Arrange
    await using var context = CreateContext();

    var service = CreateService(context);

    // Act
    var result = await service.GetById(999);

    // Assert
    Assert.Null(result);
  }
}