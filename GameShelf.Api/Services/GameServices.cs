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

  public async Task<PagedResult<Game>> GetAll(GameQueryParameters parameters)
  {
    var query = _context.Games.AsQueryable();

    if (!string.IsNullOrWhiteSpace(parameters.Genre))
    {
      query = query.Where(game =>
          game.Genre.ToLower() == parameters.Genre.ToLower());
    }

    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var search = parameters.Search.ToLower();

      query = query.Where(game =>
          game.Title.ToLower().Contains(search));
    }

    var totalItems = await query.CountAsync();

    if (!string.IsNullOrWhiteSpace(parameters.SortBy))
    {
      query = parameters.SortBy.ToLower() switch
      {
        "title" => parameters.Descending
            ? query.OrderByDescending(game => game.Title)
            : query.OrderBy(game => game.Title),

        "genre" => parameters.Descending
            ? query.OrderByDescending(game => game.Genre)
            : query.OrderBy(game => game.Genre),

        "releaseyear" => parameters.Descending
            ? query.OrderByDescending(game => game.ReleaseYear)
            : query.OrderBy(game => game.ReleaseYear),

        _ => query.OrderBy(game => game.Id)
      };
    }
    else
    {
      query = query.OrderBy(game => game.Id);
    }

    var page = parameters.Page < 1
        ? 1
        : parameters.Page;

    var pageSize = parameters.PageSize < 1
        ? 20
        : Math.Min(parameters.PageSize, 100);

    var items = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(
        totalItems / (double)pageSize);

    return new PagedResult<Game>
    {
      Items = items,
      Page = page,
      PageSize = pageSize,
      TotalItems = totalItems,
      TotalPages = totalPages
    };
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