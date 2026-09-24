using GameShelf.Api.Services;
using GameShelf.Api.DTOs;
using GameShelf.Api.Data;
using GameShelf.Api.Exceptions;

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});
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

app.UseExceptionHandler();

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

app.MapGet("/games", async (
    GameQueryParameters parameters,
    GameService gameService) =>
{
    return await gameService.GetAll(parameters);
});


app.MapGet("/games/{id}", async (int id, GameService gameService) =>
{
    var game = await gameService.GetById(id);

    if (game is null)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Game not found.");
    }

    return Results.Ok(game);
});

app.MapPost("/games", async (CreateGameRequest request, GameService gameService) =>
{
    var game = await gameService.Create(request);

    return Results.Created($"/games/{game.Id}", game);
});

app.MapPut("/games/{id}", async (int id, UpdateGameRequest request, GameService gameService) =>
{
    var game = await gameService.Update(id, request);

    if (game is null)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Game not found.");
    }

    return Results.Ok(game);
});

app.MapDelete("/games/{id}", async (int id, GameService gameService) =>
{
    bool isDeleted = await gameService.Delete(id);

    if (!isDeleted)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Game not found.");
    }

    return Results.NoContent();
});


app.Run();
