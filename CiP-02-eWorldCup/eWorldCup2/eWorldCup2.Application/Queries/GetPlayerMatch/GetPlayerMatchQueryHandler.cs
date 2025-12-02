using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eWorldCup2.Application.Queries.GetPlayerMatch
{
    public class GetPlayerMatchQueryHandler
    {
        private readonly WorldCupDbContext dbContext;
        private readonly SpecifikPlayerRound specifikPlayerRound;

        public GetPlayerMatchQueryHandler(WorldCupDbContext dbContext, SpecifikPlayerRound specifikPlayerRound)
        {
            this.dbContext = dbContext;
            this.specifikPlayerRound = specifikPlayerRound;
        }

        public async Task<Result<GetPlayerMatchResult>> HandleAsync(GetPlayerMatchQuery query)
        {
            var players = await dbContext.Players.ToListAsync();
            var result = specifikPlayerRound.GetSpecificRound(
                players,
                query.NumberOfPlayers,
                query.PlayerId,
                query.SpecificRound);

            if (!result.IsSuccess)
            {
                return Result<GetPlayerMatchResult>.Failure(result.Error!);
            }

            var (player, opponent) = result.Value;
            var queryResult = new GetPlayerMatchResult(player, opponent);
            return Result<GetPlayerMatchResult>.Success(queryResult);
        }
    }
}
