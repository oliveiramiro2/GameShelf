using GameShelf.Api.Data;
using GameShelf.Api.DTOs;
using GameShelf.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShelf.Api.Services;

public class GameService
{
  private readonly GameShelfDbContext _context;
  private readonly ILogger<GameService> _logger;

  public GameService(
      GameShelfDbContext context,
      ILogger<GameService> logger)
  {
    _context = context;
    _logger = logger;
  }

  public async Task<List<Game>> GetAll()
  {
    return await _context.Games.ToListAsync();
  }

  public async Task<Game?> GetById(int id)
  {
    return await _context.Games.FirstOrDefaultAsync(game => game.Id == id);
  }

  public async Task<Game> Create(CreateGameRequest request)
  {
    var game = new Game
    {
      Title = request.Title,
      Genre = request.Genre,
      ReleaseYear = request.ReleaseYear
    };

    _context.Games.Add(game);
    await _context.SaveChangesAsync();

    _logger.LogInformation(
        "Game created successfully. GameId: {GameId}, Title: {Title}",
        game.Id,
        game.Title);

    return game;
  }

  public async Task<Game?> Update(int id, UpdateGameRequest request)
  {
    var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

    if (game is null)
    {
      return null;
    }

    game.Title = request.Title;
    game.Genre = request.Genre;
    game.ReleaseYear = request.ReleaseYear;

    await _context.SaveChangesAsync();

    return game;
  }

  public async Task<bool> Delete(int id)
  {
    var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

    if (game is null)
    {
      return false;
    }

    _context.Games.Remove(game);
    await _context.SaveChangesAsync();

    _logger.LogInformation(
        "Game deleted successfully. GameId: {GameId}, Title: {Title}",
        game.Id,
        game.Title);

    return true;
  }
}