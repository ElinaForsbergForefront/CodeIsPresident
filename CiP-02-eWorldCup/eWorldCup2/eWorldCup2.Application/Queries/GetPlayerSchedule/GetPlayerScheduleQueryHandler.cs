using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eWorldCup2.Application.Queries.GetPlayerSchedule
{
    public class GetPlayerScheduleQueryHandler
    {
        private readonly WorldCupDbContext dbContext;
        private readonly SpecifikPlayerRound specifikPlayerRound;

        public GetPlayerScheduleQueryHandler(WorldCupDbContext dbContext, SpecifikPlayerRound specifikPlayerRound)
        {
            this.dbContext = dbContext;
            this.specifikPlayerRound = specifikPlayerRound;
        }

        public async Task<Result<GetPlayerScheduleResult>> HandleAsync(GetPlayerScheduleQuery query)
        {
            var players = await dbContext.Players.ToListAsync();
            var matches = new List<PlayerScheduleMatch>();

            for (int specificRound = 1; specificRound < query.NumberOfPlayers; specificRound++)
            {
                var result = specifikPlayerRound.GetSpecificRound(
                    players,
                    query.NumberOfPlayers,
                    query.PlayerId,
                    specificRound);

                if (!result.IsSuccess)
                {
                    return Result<GetPlayerScheduleResult>.Failure(result.Error!);
                }

                var (player, opponent) = result.Value;
                matches.Add(new PlayerScheduleMatch(specificRound, player, opponent));
            }

            var queryResult = new GetPlayerScheduleResult(matches);
            return Result<GetPlayerScheduleResult>.Success(queryResult);
        }
    }
}
