using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;

namespace eWorldCup2.Application.Commands.DeletePlayer
{
    public class DeletePlayerCommandHandler
    {
        private readonly WorldCupDbContext dbContext;

        public DeletePlayerCommandHandler(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<DeletePlayerResult>> HandleAsync(DeletePlayerCommand command)
        {
            var player = await dbContext.Players.FindAsync(command.PlayerId);

            if (player == null)
            {
                return Result<DeletePlayerResult>.Failure(
                    new Error("PLAYER_NOT_FOUND", "Spelare hittades inte."));
            }

            dbContext.Players.Remove(player);
            await dbContext.SaveChangesAsync();

            var result = new DeletePlayerResult(true);
            return Result<DeletePlayerResult>.Success(result);
        }
    }
}
