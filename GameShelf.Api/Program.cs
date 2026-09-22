using GameShelf.Api.Models;
using GameShelf.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/hello", () =>
{
    return ".NET API!";
});

app.MapGet("/games", (GameService gameService) =>
{
    return gameService.GetAll();
});


app.MapGet("/games/{id}", (int id, GameService gameService) =>
{
    var game = gameService.GetById(id);

    if (game is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
});


app.Run();
