using GameShelf.Api.Services;
using GameShelf.Api.DTOs;
using GameShelf.Api.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// add scope's to use in container of build for serve end point
builder.Services.AddScoped<GameService>();

// add validations
builder.Services.AddValidation();

// add conection with DB 
builder.Services.AddDbContext<GameShelfDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("GameShelf")));

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

app.MapPost("/games", async (CreateGameRequest request, GameService gameService) =>
{
    var game = await gameService.Create(request);

    return Results.Created($"/games/{game.Id}", game);
});


app.Run();
