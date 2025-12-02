using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.Models;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eWorldCup2.Application.Commands.CreatePlayer
{
    public class CreatePlayerCommandHandler
    {
        private readonly WorldCupDbContext dbContext;

        public CreatePlayerCommandHandler(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<CreatePlayerResult>> HandleAsync(CreatePlayerCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result<CreatePlayerResult>.Failure(
                    new Error("INVALID_NAME", "Spelarnamn får inte vara tomt."));
            }

            if (await dbContext.Players.AnyAsync(p => p.Name == command.Name))
            {
                return Result<CreatePlayerResult>.Failure(
                    new Error("PLAYER_EXISTS", $"Spelare '{command.Name}' finns redan."));
            }

            var player = new Player(0, command.Name);
            dbContext.Players.Add(player);
            await dbContext.SaveChangesAsync();

            var result = new CreatePlayerResult(player);
            return Result<CreatePlayerResult>.Success(result);
        }
    }
}
