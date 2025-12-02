using eWorldCup2.Domain.Models;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Commands.PlayMove
{
    public class PlayMoveCommandHandler
    {
        private readonly WorldCupDbContext dbContext;

        public PlayMoveCommandHandler(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<PlayMoveResult>> HandleAsync(PlayMoveCommand command)
        {
            // Validate tournament
            var tournament = await dbContext.Tournaments
                .Include(t => t.Matches)
                .FirstOrDefaultAsync(t => t.Id == command.TournamentId);

            if (tournament == null)
            {
                return Result<PlayMoveResult>.Failure(
                    new Error("TOURNAMENT_NOT_FOUND", "Tournament not found."));
            }

            if (tournament.IsCompleted)
            {
                return Result<PlayMoveResult>.Failure(
                    new Error("TOURNAMENT_COMPLETED", "Tournament is already completed."));
            }

            // Find current match
            var currentMatch = tournament.Matches
                .FirstOrDefault(m => m.Round == tournament.CurrentRound &&
                                    (m.Player1Id == command.PlayerId || m.Player2Id == command.PlayerId));

            if (currentMatch == null)
            {
                return Result<PlayMoveResult>.Failure(
                    new Error("MATCH_NOT_FOUND", "No match found for player in current round."));
            }

            if (currentMatch.IsCompleted)
            {
                return Result<PlayMoveResult>.Failure(
                    new Error("MATCH_COMPLETED", "Match is already completed."));
            }

            // Parse move
            int playerMove = command.Move.ToLower() switch
            {
                "rock" => 0,
                "paper" => 1,
                "scissors" => 2,
                _ => -1
            };

            if (playerMove == -1)
            {
                return Result<PlayMoveResult>.Failure(
                    new Error("INVALID_MOVE", "Invalid move. Use 'rock', 'paper', or 'scissors'."));
            }

            // Execute game
            var random = new Random();
            var opponentMove = random.Next(0, 3);
            var gameResult = DetermineWinner(playerMove, opponentMove);
            var isPlayer1 = currentMatch.Player1Id == command.PlayerId;

            // Update match
            dbContext.Entry(currentMatch).State = EntityState.Detached;
            var updatedMatch = UpdateMatch(currentMatch, isPlayer1, gameResult);

            dbContext.Matches.Update(updatedMatch);
            await dbContext.SaveChangesAsync();

            // Get players
            var player = await dbContext.Players.FindAsync(command.PlayerId);
            var opponentId = isPlayer1 ? updatedMatch.Player2Id : updatedMatch.Player1Id;
            var opponent = await dbContext.Players.FindAsync(opponentId);

            if (player == null || opponent == null)
            {
                return Result<PlayMoveResult>.Failure(
                    new Error("PLAYER_NOT_FOUND", "Player or opponent not found."));
            }

            var result = new PlayMoveResult(
                updatedMatch, player, opponent, isPlayer1,
                playerMove, opponentMove, gameResult
            );

            return Result<PlayMoveResult>.Success(result);
        }

        private Match UpdateMatch(Match match, bool isPlayer1, int gameResult)
        {
            var updatedMatch = match;

            if (gameResult == 1)
            {
                updatedMatch = isPlayer1
                    ? updatedMatch with { Player1Wins = updatedMatch.Player1Wins + 1 }
                    : updatedMatch with { Player2Wins = updatedMatch.Player2Wins + 1 };
            }
            else if (gameResult == -1)
            {
                updatedMatch = isPlayer1
                    ? updatedMatch with { Player2Wins = updatedMatch.Player2Wins + 1 }
                    : updatedMatch with { Player1Wins = updatedMatch.Player1Wins + 1 };
            }

            if (updatedMatch.Player1Wins == 2 || updatedMatch.Player2Wins == 2)
            {
                updatedMatch = updatedMatch with
                {
                    IsCompleted = true,
                    WinnerId = updatedMatch.Player1Wins == 2 ? updatedMatch.Player1Id : updatedMatch.Player2Id
                };
            }

            return updatedMatch;
        }

        private static int DetermineWinner(int move1, int move2)
        {
            if (move1 == move2) return 0;
            return (move1, move2) switch
            {
                (0, 2) => 1,
                (1, 0) => 1,
                (2, 1) => 1,
                _ => -1
            };
        }
    }
}
