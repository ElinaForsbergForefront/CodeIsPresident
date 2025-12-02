
using eWorldCup2.Application;
using eWorldCup2.Application.Commands.AdvanceRound;
using eWorldCup2.Application.Commands.CreatePlayer;
using eWorldCup2.Application.Commands.DeletePlayer;
using eWorldCup2.Application.Commands.PlayMove;
using eWorldCup2.Application.Commands.StartTournament;
using eWorldCup2.Application.Queries.GetAllPlayers;
using eWorldCup2.Application.Queries.GetFinalResults;
using eWorldCup2.Application.Queries.GetMaxRounds;
using eWorldCup2.Application.Queries.GetPlayerMatch;
using eWorldCup2.Application.Queries.GetPlayerMatchByName;
using eWorldCup2.Application.Queries.GetPlayerSchedule;
using eWorldCup2.Application.Queries.GetRemainingPairs;
using eWorldCup2.Application.Queries.GetRoundPairs;
using eWorldCup2.Application.Queries.GetTournamentStatus;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// ✅ Lägg till CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // <-- din React-app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RoundRobinService>();
builder.Services.AddScoped<MaxRoundsCalculator>();
builder.Services.AddScoped<RemainingPairsCalculator>();
builder.Services.AddScoped<SpecifikPlayerRound>();
//builder.Services.AddScoped<TournamentService>();

// Add command handlers
builder.Services.AddScoped<StartTournamentCommandHandler>();
builder.Services.AddScoped<PlayMoveCommandHandler>();
builder.Services.AddScoped<AdvanceRoundCommandHandler>();

// Add query handlers
builder.Services.AddScoped<GetTournamentStatusQueryHandler>();
builder.Services.AddScoped<GetFinalResultsQueryHandler>();


// Player command handlers
builder.Services.AddScoped<CreatePlayerCommandHandler>();
builder.Services.AddScoped<DeletePlayerCommandHandler>();

// RoundRobin query handlers
builder.Services.AddScoped<GetAllPlayersQueryHandler>();
builder.Services.AddScoped<GetRoundPairsQueryHandler>();
builder.Services.AddScoped<GetMaxRoundsQueryHandler>();
builder.Services.AddScoped<GetRemainingPairsQueryHandler>();
builder.Services.AddScoped<GetPlayerMatchQueryHandler>();
builder.Services.AddScoped<GetPlayerScheduleQueryHandler>();
builder.Services.AddScoped<GetPlayerMatchByNameQueryHandler>();

builder.Services.AddDbContext<WorldCupDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Använd CORS före controllers
app.UseCors("DevCors");

app.UseAuthorization();

app.MapControllers();

app.Run();

