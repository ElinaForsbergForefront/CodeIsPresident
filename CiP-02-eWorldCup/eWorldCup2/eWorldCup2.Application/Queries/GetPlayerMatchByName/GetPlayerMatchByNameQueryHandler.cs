using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eWorldCup2.Application.Queries.GetPlayerMatchByName
{
    public class GetPlayerMatchByNameQueryHandler
    {
        private readonly WorldCupDbContext dbContext;
        private readonly SpecifikPlayerRound specifikPlayerRound;

        public GetPlayerMatchByNameQueryHandler(WorldCupDbContext dbContext, SpecifikPlayerRound specifikPlayerRound)
        {
            this.dbContext = dbContext;
            this.specifikPlayerRound = specifikPlayerRound;
        }

        public async Task<Result<GetPlayerMatchByNameResult>> HandleAsync(GetPlayerMatchByNameQuery query)
        {
            var players = await dbContext.Players.ToListAsync();
            var player = players.Find(p => p.Name == query.PlayerName);

            if (player is null)
            {
                return Result<GetPlayerMatchByNameResult>.Failure(
                    new Error("PLAYER_NOT_FOUND", $"Spelare med namn '{query.PlayerName}' hittades inte."));
            }

            var result = specifikPlayerRound.GetSpecificRound(
                players,
                players.Count,
                player.Id - 1,
                query.SpecificRound);

            if (!result.IsSuccess)
            {
                return Result<GetPlayerMatchByNameResult>.Failure(result.Error!);
            }

            var (foundPlayer, opponent) = result.Value;
            var queryResult = new GetPlayerMatchByNameResult(foundPlayer, opponent);
            return Result<GetPlayerMatchByNameResult>.Success(queryResult);
        }
    }
}
