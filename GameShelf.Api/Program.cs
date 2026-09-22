using GameShelf.Api.Models;
using GameShelf.Api.Services;
using GameShelf.Api.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// add singletons to use in container of build for serve end point
builder.Services.AddSingleton<GameService>();

// add validations
builder.Services.AddValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "GameShelf API v1");
    });
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

app.MapPost("/games", (
    CreateGameRequest request,
    GameService gameService) =>
{
    var game = gameService.Create(request);

    return Results.Created($"/games/{game.Id}", game);
});


app.Run();
