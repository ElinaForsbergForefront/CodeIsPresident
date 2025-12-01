
using eWorldCup2.Application;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using eWorldCup2.Application.Services;


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
builder.Services.AddScoped<TournamentService>();


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

