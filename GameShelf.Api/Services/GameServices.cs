using GameShelf.Api.Models;
using GameShelf.Api.DTOs;

namespace GameShelf.Api.Services;

public class GameService
{
  private readonly List<Game> _games = new()
  {
    new Game
    {
        Id = 1,
        Title = "Hollow Knight",
        Genre = "Metroidvania",
        ReleaseYear = 2017
    },
    new Game
    {
        Id = 2,
        Title = "Celeste",
        Genre = "Platformer",
        ReleaseYear = 2018
    }
  };

  public List<Game> GetAll()
  {
    return _games;
  }

  public Game? GetById(int id)
  {
    return _games.FirstOrDefault(game => game.Id == id);
  }

  public Game Create(CreateGameRequest request)
  {
    var nextId = _games.Count == 0
        ? 1
        : _games.Max(game => game.Id) + 1;

    var game = new Game
    {
      Id = nextId,
      Title = request.Title,
      Genre = request.Genre,
      ReleaseYear = request.ReleaseYear
    };

    _games.Add(game);

    return game;
  }
}