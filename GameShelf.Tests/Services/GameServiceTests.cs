using GameShelf.Api.Data;
using GameShelf.Api.Models;
using GameShelf.Api.Services;
using GameShelf.Api.DTOs;
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

  [Fact]
  public async Task Create_ShouldCreateAndPersistGame()
  {
    // Arrange
    await using var context = CreateContext();

    var service = CreateService(context);

    var request = new CreateGameRequest
    {
      Title = "Hollow Knight",
      Genre = "Metroidvania",
      ReleaseYear = 2017
    };

    // Act
    var result = await service.Create(request);

    // Assert
    Assert.NotNull(result);
    Assert.True(result.Id > 0);
    Assert.Equal("Hollow Knight", result.Title);
    Assert.Equal("Metroidvania", result.Genre);
    Assert.Equal(2017, result.ReleaseYear);

    var savedGame = await context.Games.FirstOrDefaultAsync();

    Assert.NotNull(savedGame);
    Assert.Equal(result.Id, savedGame.Id);
  }

  [Fact]
  public async Task Update_ShouldUpdateAndPersistGame_WhenGameExists()
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

    var request = new UpdateGameRequest
    {
      Title = "Hollow Knight: Silksong",
      Genre = "Metroidvania",
      ReleaseYear = 2025
    };

    // Act
    var result = await service.Update(1, request);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(1, result.Id);
    Assert.Equal("Hollow Knight: Silksong", result.Title);
    Assert.Equal("Metroidvania", result.Genre);
    Assert.Equal(2025, result.ReleaseYear);

    var savedGame = await context.Games.FirstOrDefaultAsync(game => game.Id == 1);

    Assert.NotNull(savedGame);
    Assert.Equal("Hollow Knight: Silksong", savedGame.Title);
    Assert.Equal(2025, savedGame.ReleaseYear);
  }

  [Fact]
  public async Task Update_ShouldReturnNull_WhenGameDoesNotExist()
  {
    // Arrange
    await using var context = CreateContext();

    var service = CreateService(context);

    var request = new UpdateGameRequest
    {
      Title = "Hollow Knight",
      Genre = "Metroidvania",
      ReleaseYear = 2017
    };

    // Act
    var result = await service.Update(999, request);

    // Assert
    Assert.Null(result);
  }
}