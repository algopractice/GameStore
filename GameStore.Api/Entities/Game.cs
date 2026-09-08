using System.Security.Cryptography.X509Certificates;

namespace GameStore.Api.Entities;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // 1 : 1
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

}