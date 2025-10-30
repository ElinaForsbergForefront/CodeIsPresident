using eWorldCup2.Application;
using eWorldCup2.Domain;

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







// ---- 1️⃣ Skapa testdata ----
var players = new List<Player>
{
          new(1, "Alice"),
            new(2, "Bob"),
            new(3, "Charlie"),
            new(4, "Diana"),
            new(5, "Ethan"),
            new(6, "Fiona"),
            new(7, "George"),
            new(8, "Hannah"),
            new(9, "Isaac"),
            new(10, "Julia"),
            new(11, "Kevin"),
            new(12, "Laura"),
            new(13, "Michael"),
            new(14, "Nina"),
            new(15, "Oscar"),
            new(16, "Paula"),
            new(17, "Quentin"),
            new(18, "Rachel"),
            new(19, "Samuel"),
            new(20, "Tina")
};

// ---- 2️⃣ Be användaren välja runda ----
Console.WriteLine("Välkommen till eWorldCup2-turneringen!");
Console.WriteLine($"Antal deltagare: {players.Count}");
Console.WriteLine($"Ange runda (1 - {players.Count - 1}): ");

if (!int.TryParse(Console.ReadLine(), out int round))
{
    Console.WriteLine("Fel: ogiltigt nummer.");
    return;
}

// ---- 3️⃣ Skapa service och beräkna par ----
var service = new RoundRobinService();
var result = service.GetRoundPairs(players, round);

// ---- 4️⃣ Railway: kontrollera resultat ----
if (!result.IsSuccess)
{
    Console.WriteLine($"⚠️ Fel: {result.Error!.Message}");
    return;
}

// ---- 5️⃣ Skriv ut resultatet ----
Console.WriteLine($"\n🎯 Runda {round}:");
foreach (var (a, b) in result.Value!)
{
    Console.WriteLine($"{a.Name} vs {b.Name}");
}






// ---- Max antal rundor ----
Console.WriteLine("_________________________________________");
Console.WriteLine("_________________________________________");
Console.WriteLine("Skriv ett jämt antal spelare för att räkna ut max antal rundor.");

if (!int.TryParse(Console.ReadLine(), out int playersForMax))
{
    Console.WriteLine("Fel: ogiltigt nummer.");
    return;
}

if (playersForMax % 2 != 0)
{
    Console.WriteLine("Fel: Antalet deltagare måste vara jämnt.");
    return;
}

var maxCalulator = new MaxRoundsCalculator();
var maxResult = maxCalulator.CalculateMaxRounds(playersForMax);

if (!maxResult.IsSuccess)
{
    Console.WriteLine($"Fel: {maxResult.Error!.Message}");
    return;
}

Console.WriteLine($"\nMax antal rundor för {playersForMax} spelare är: {maxResult.Value}.");


Console.WriteLine("_________________________________________");
Console.WriteLine("_________________________________________");
// ---- Räkna ut återstående par ----
Console.WriteLine("Skriv ett jämt antal spelare");
if (!int.TryParse(Console.ReadLine(), out int playersForRemaining))
{
    Console.WriteLine("Fel: ogiltigt nummer.");
    return;
}

Console.WriteLine("Skriv antal genomförda rundor:");
if (!int.TryParse(Console.ReadLine(), out int completedRounds))
{
    Console.WriteLine("Fel: ogiltigt nummer.");
    return;
}

var remainingCalculator = new RemainingPairsCalculator();
var remainingResult = remainingCalculator.CalculateRemainingPairs(playersForRemaining, completedRounds);

if (!remainingResult.IsSuccess)
{
    Console.WriteLine($"Fel: {remainingResult.Error!.Message}");
    return;
}
Console.WriteLine($"\nAntal återstående par att spela är: {remainingResult.Value}.");


Console.WriteLine("_________________________________________");
Console.WriteLine("_________________________________________");
// ---- Specifik spelare och runda ----

Console.WriteLine("Skriv antal spelare:");
if (!int.TryParse(Console.ReadLine(), out int nPlayers))
{
    Console.WriteLine("Fel: ogiltigt nummer.");
    return;
}

Console.WriteLine("Skriv spelarindex (0-baserat):");

if (!int.TryParse(Console.ReadLine(), out int playerIndex))
{
    Console.WriteLine("Fel: ogiltigt nummer.");
    return;
}

Console.WriteLine("Skriv runda:");
if (!int.TryParse(Console.ReadLine(), out int roundNumber))
{
    Console.WriteLine("Fel: ogiltigt nummer.");
    return;
}

var specificRoundService = new SpecifikPlayerRound();
var specificResult = specificRoundService.GetSpecificRound(players, nPlayers, playerIndex, roundNumber);

if (!specificResult.IsSuccess)
{
    Console.WriteLine($"Fel: {specificResult.Error!.Message}");
    return;
}

var (playerName, opponentName) = specificResult.Value!;
Console.WriteLine($"\nI runda {roundNumber} möter {playerName} mot {opponentName}.");


