using GameStore.Api.Data;
using GameStore.Api.DTOs;
using GameStore.Api.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string GetRouteName = "GetGame";
    private static readonly List<GameDto> games = [
        new(1, "Street Fighter", "Fighting", 19.99M, new DateOnly(1992, 7, 1)),
        new(2, "Final Fantasy", "RPG", 19.99M, new DateOnly(1992, 7, 1)),
        new(3, "Contra", "Shooting", 19.99M, new DateOnly(1992, 7, 1)),
        new(4, "Super Contra", "Shooting", 19.99M, new DateOnly(1992, 7, 1)),
        new(5, "Super Mario", "Jumping", 19.99M, new DateOnly(1992, 7, 1))
        ];

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation();

        // Get /Games . MapGet(path,handler)
        group.MapGet("/", () => games);

        // TODO : Don't see GetRouteName in the header
        // Get / Game/id
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        )
        .WithName(GetRouteName);

        /*
                // Post - using list
                group.MapPost("/", (CreateGameDto newGame) =>
                {
                    GameDto game = new(
                        games.Count + 1, // fix this, if id 1 is deleted, its repeting id 5
                        newGame.Name,
                        newGame.Genre,
                        newGame.Price,
                        newGame.ReleaseDate
                        );
                    games.Add(game);
                    return Results.CreatedAtRoute(GetRouteName, new { id = game.Id }, game);
                });
        */

        // Post - using db

        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                ReleaseDate = newGame.ReleaseDate
            };
            dbContext.Games.Add(game);


            // ------------------------------------------------------------------------------------------------------------------------------------------------
            // TODO : Check why the post id and Genre id is populating with 0
            // TODO : Also after population it is not returning in GET 
            // ------------------------------------------------------------------------------------------------------------------------------------------------

            return Results.CreatedAtRoute(GetRouteName, new { id = game.Id }, game);
        });


        //        .WithParameterValidation(); -- can 

        // put

        group.MapPut("/{id}", (int id, UpdateGameDto updateGameDto) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new GameDto(
                id,
                updateGameDto.Name,
                updateGameDto.Genre,
                updateGameDto.Price,
                updateGameDto.ReleaseDate
            );
            return Results.NoContent();
        });

        // del
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });

        return group;
    }
}