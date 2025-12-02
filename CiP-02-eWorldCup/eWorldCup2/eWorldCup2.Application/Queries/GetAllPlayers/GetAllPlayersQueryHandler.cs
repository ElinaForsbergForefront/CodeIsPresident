using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace eWorldCup2.Application.Queries.GetAllPlayers
{
    public class GetAllPlayersQueryHandler
    {
        private readonly WorldCupDbContext dbContext;

        public GetAllPlayersQueryHandler(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<GetAllPlayersResult>> HandleAsync(GetAllPlayersQuery query)
        {
            var players = await dbContext.Players.ToListAsync();
            var result = new GetAllPlayersResult(players);
            return Result<GetAllPlayersResult>.Success(result);
        }
    }
}
