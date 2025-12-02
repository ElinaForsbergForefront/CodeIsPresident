using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Queries.GetTournamentStatus
{
    public class GetTournamentStatusQueryHandler
    {
        private readonly WorldCupDbContext dbContext;

        public GetTournamentStatusQueryHandler(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<TournamentStatusResult>> HandleAsync(GetTournamentStatusQuery query)
        {
            // Get tournament with matches
            var tournament = await dbContext.Tournaments
                .Include(t => t.Matches)
                .FirstOrDefaultAsync(t => t.Id == query.TournamentId);

            if (tournament == null)
            {
                return Result<TournamentStatusResult>.Failure(
                    new Error("TOURNAMENT_NOT_FOUND", "Tournament not found."));
            }

            // Get player
            var player = await dbContext.Players.FindAsync(query.PlayerId);
            if (player == null)
            {
                return Result<TournamentStatusResult>.Failure(
                    new Error("PLAYER_NOT_FOUND", "Player not found."));
            }

            // Find current match
            var currentMatch = tournament.Matches
                .FirstOrDefault(m => m.Round == tournament.CurrentRound &&
                                    (m.Player1Id == query.PlayerId || m.Player2Id == query.PlayerId));

            if (currentMatch == null)
            {
                return Result<TournamentStatusResult>.Failure(
                    new Error("MATCH_NOT_FOUND", "No match found for player in current round."));
            }

            // Get opponent
            var isPlayer1 = currentMatch.Player1Id == query.PlayerId;
            var opponentId = isPlayer1 ? currentMatch.Player2Id : currentMatch.Player1Id;
            var opponent = await dbContext.Players.FindAsync(opponentId);

            if (opponent == null)
            {
                return Result<TournamentStatusResult>.Failure(
                    new Error("OPPONENT_NOT_FOUND", "Opponent not found."));
            }

            // Get all matches in current round
            var allMatchesInRound = tournament.Matches
                .Where(m => m.Round == tournament.CurrentRound)
                .ToList();

            var result = new TournamentStatusResult(
                tournament, currentMatch, player, opponent, isPlayer1, allMatchesInRound
            );

            return Result<TournamentStatusResult>.Success(result);
        }
    }
}
