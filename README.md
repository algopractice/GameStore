Minimal Api using dot net 8 - based on this [tutorial](https://youtu.be/AhAxLiGC7Pc)
Still in progress

# Building DB
Generate a fresh database through migration files using one of two standard approaches:

### Approach 1: Using the EF CLI Tool

```bash
dotnet ef database update
```
This command reads your EF Core migration history and builds a brand-new local `GameStore.db` file matching your exact schema.


### Approach 2: Automatic Creation on Startup (Recommended)

`Program.cs` can be configured to apply migrations automatically when the application boots up, don't need to run a manual command while using this:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    db.Database.Migrate();
}
```
This part is already implemented in `app.MigrateDb()` extension method for this code.

Note :
When they run `dotnet run` (or launch it from Rider), the app automatically detects that `GameStore.db` is missing and creates it on the fly.

