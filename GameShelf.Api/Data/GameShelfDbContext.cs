using GameShelf.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameShelf.Api.Data;

public class GameShelfDbContext : DbContext
{
  public GameShelfDbContext(DbContextOptions<GameShelfDbContext> options)
      : base(options)
  {
  }

  public DbSet<Game> Games => Set<Game>();
}