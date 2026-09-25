using GameShelf.Api.Data;
using GameShelf.Api.Models;
using GameShelf.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameShelf.Tests.Services;

public class GameServiceTests
{
  [Fact]
  public async Task GetById_ShouldReturnGame_WhenGameExists()
  {
    // Arrange
    var options = new DbContextOptionsBuilder<GameShelfDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    await using var context = new GameShelfDbContext(options);

    context.Games.Add(new Game
    {
      Id = 1,
      Title = "Hollow Knight",
      Genre = "Metroidvania",
      ReleaseYear = 2017
    });

    await context.SaveChangesAsync();

    using var loggerFactory = LoggerFactory.Create(builder => { });
    var logger = loggerFactory.CreateLogger<GameService>();

    var service = new GameService(context, logger);

    // Act
    var result = await service.GetById(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Hollow Knight", result.Title);
  }
}