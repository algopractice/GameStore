using GameStore.Api.Data;
using GameStore.Api.DTOs;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.SignalR.Protocol;



var builder = WebApplication.CreateBuilder(args);
// Configure services for the application
//builder.Services.AddValidations();

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);


//builder.Services.AddTransient

// configure http piepline from below onwards
var app = builder.Build();
app.MapGameEndpoints();
app.MigrateDb();
app.Run();


