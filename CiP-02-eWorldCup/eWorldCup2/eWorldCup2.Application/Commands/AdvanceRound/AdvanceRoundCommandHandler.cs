using eWorldCup2.Domain.Models;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Commands.AdvanceRound
{
    public class AdvanceRoundCommandHandler
    {
        private readonly WorldCupDbContext dbContext;
        private readonly RoundRobinService roundRobinService;

        public AdvanceRoundCommandHandler(
            WorldCupDbContext dbContext,
            RoundRobinService roundRobinService)
        {
            this.dbContext = dbContext;
            this.roundRobinService = roundRobinService;
        }

        public async Task<Result<AdvanceRoundResult>> HandleAsync(AdvanceRoundCommand command)
        {
            // Validate tournament
            var tournament = await dbContext.Tournaments
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == command.TournamentId);

            if (tournament == null)
            {
                return Result<AdvanceRoundResult>.Failure(
                    new Error("TOURNAMENT_NOT_FOUND", "Tournament not found."));
            }

            if (tournament.IsCompleted)
            {
                return Result<AdvanceRoundResult>.Failure(
                    new Error("TOURNAMENT_COMPLETED", "Tournament is already completed."));
            }

            // Simulate incomplete matches
            await SimulateIncompleteMatchesAsync(command.TournamentId, tournament.CurrentRound);

            // Check if tournament is complete
            var maxRounds = tournament.NumberOfPlayers - 1;
            if (tournament.CurrentRound >= maxRounds)
            {
                var completedTournament = await CompleteTournamentAsync(tournament);
                var completedResult = new AdvanceRoundResult(completedTournament, null, null, true);
                return Result<AdvanceRoundResult>.Success(completedResult);
            }

            // Advance to next round
            var nextRound = tournament.CurrentRound + 1;
            var advancedTournament = tournament with { CurrentRound = nextRound };

            var allPlayers = await dbContext.Players
                .Take(tournament.NumberOfPlayers)
                .ToListAsync();

            var pairingResult = roundRobinService.GetRoundPairs(allPlayers, nextRound);

            if (!pairingResult.IsSuccess)
            {
                return Result<AdvanceRoundResult>.Failure(pairingResult.Error!);
            }

            var newMatches = pairingResult.Value!.Select(pair => new Match
            {
                TournamentId = tournament.Id,
                Round = nextRound,
                Player1Id = pair.Item1.Id,
                Player2Id = pair.Item2.Id,
                Player1Wins = 0,
                Player2Wins = 0,
                IsCompleted = false,
                WinnerId = null
            }).ToList();

            dbContext.Matches.AddRange(newMatches);
            dbContext.Tournaments.Update(advancedTournament);
            await dbContext.SaveChangesAsync();

            var result = new AdvanceRoundResult(advancedTournament, newMatches, allPlayers, false);
            return Result<AdvanceRoundResult>.Success(result);
        }

        private async Task SimulateIncompleteMatchesAsync(int tournamentId, int currentRound)
        {
            var currentRoundMatches = await dbContext.Matches
                .Where(m => m.TournamentId == tournamentId && m.Round == currentRound)
                .ToListAsync();

            var random = new Random();
            var updatedMatches = new List<Match>();

            foreach (var match in currentRoundMatches.Where(m => !m.IsCompleted))
            {
                var entry = dbContext.Entry(match);
                if (entry.State != EntityState.Detached)
                {
                    entry.State = EntityState.Detached;
                }

                var updatedMatch = SimulateMatch(match, random);
                updatedMatches.Add(updatedMatch);
            }

            if (updatedMatches.Any())
            {
                dbContext.Matches.UpdateRange(updatedMatches);
                await dbContext.SaveChangesAsync();
            }
        }

        private Match SimulateMatch(Match match, Random random)
        {
            var updatedMatch = match;

            while (updatedMatch.Player1Wins < 2 && updatedMatch.Player2Wins < 2)
            {
                var move1 = random.Next(0, 3);
                var move2 = random.Next(0, 3);
                var result = DetermineWinner(move1, move2);

                if (result == 1)
                    updatedMatch = updatedMatch with { Player1Wins = updatedMatch.Player1Wins + 1 };
                else if (result == -1)
                    updatedMatch = updatedMatch with { Player2Wins = updatedMatch.Player2Wins + 1 };
            }

            return updatedMatch with
            {
                IsCompleted = true,
                WinnerId = updatedMatch.Player1Wins == 2 ? updatedMatch.Player1Id : updatedMatch.Player2Id
            };
        }

        private async Task<Tournament> CompleteTournamentAsync(Tournament tournament)
        {
            var completedTournament = tournament with { IsCompleted = true };
            dbContext.Tournaments.Update(completedTournament);
            await dbContext.SaveChangesAsync();
            return completedTournament;
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
