using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eWorldCup2.Application.Queries.GetRoundPairs
{
    public class GetRoundPairsQueryHandler
    {
        private readonly WorldCupDbContext dbContext;
        private readonly RoundRobinService roundRobinService;

        public GetRoundPairsQueryHandler(WorldCupDbContext dbContext, RoundRobinService roundRobinService)
        {
            this.dbContext = dbContext;
            this.roundRobinService = roundRobinService;
        }

        public async Task<Result<GetRoundPairsResult>> HandleAsync(GetRoundPairsQuery query)
        {
            var players = await dbContext.Players.ToListAsync();
            var result = roundRobinService.GetRoundPairs(players, query.SpecificRound);

            if (!result.IsSuccess)
            {
                return Result<GetRoundPairsResult>.Failure(result.Error!);
            }

            var lines = new List<string>();
            foreach (var (a, b) in result.Value!)
            {
                lines.Add($"{a.Name} vs {b.Name}");
            }

            var queryResult = new GetRoundPairsResult(lines);
            return Result<GetRoundPairsResult>.Success(queryResult);
        }
    }
}
