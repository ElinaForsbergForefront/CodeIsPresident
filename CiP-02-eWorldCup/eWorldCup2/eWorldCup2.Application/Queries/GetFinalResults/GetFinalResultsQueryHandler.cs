using eWorldCup2.Domain.Models;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Queries.GetFinalResults
{
    public class GetFinalResultsQueryHandler
    {
        private readonly WorldCupDbContext dbContext;

        public GetFinalResultsQueryHandler(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<FinalResultsResult>> HandleAsync(GetFinalResultsQuery query)
        {
            var tournament = await dbContext.Tournaments
                .Include(t => t.Matches)
                .FirstOrDefaultAsync(t => t.Id == query.TournamentId);

            if (tournament == null)
            {
                return Result<FinalResultsResult>.Failure(
                    new Error("TOURNAMENT_NOT_FOUND", "Tournament not found."));
            }

            if (!tournament.IsCompleted)
            {
                return Result<FinalResultsResult>.Failure(
                    new Error("TOURNAMENT_NOT_COMPLETED", "Tournament is not yet completed."));
            }

            var scoreboard = await CalculateScoreboardAsync(tournament);
            var result = new FinalResultsResult(tournament, scoreboard);

            return Result<FinalResultsResult>.Success(result);
        }

        private async Task<List<ScoreboardEntry>> CalculateScoreboardAsync(Tournament tournament)
        {
            var playerStats = new Dictionary<int, (string Name, int Wins, int Losses)>();
            var allPlayers = await dbContext.Players
                .Where(p => p.Id <= tournament.NumberOfPlayers)
                .ToDictionaryAsync(p => p.Id, p => p.Name);

            foreach (var match in tournament.Matches.Where(m => m.IsCompleted))
            {
                UpdatePlayerStats(playerStats, allPlayers, match);
            }

            return playerStats
                .OrderByDescending(p => p.Value.Wins)
                .ThenBy(p => p.Value.Losses)
                .Select((p, index) => new ScoreboardEntry(
                    Rank: index + 1,
                    PlayerId: p.Key,
                    PlayerName: p.Value.Name,
                    Wins: p.Value.Wins,
                    Losses: p.Value.Losses
                ))
                .ToList();
        }

        private void UpdatePlayerStats(
            Dictionary<int, (string Name, int Wins, int Losses)> playerStats,
            Dictionary<int, string> allPlayers,
            Match match)
        {
            var player1Id = match.Player1Id;
            var player2Id = match.Player2Id;

            if (!playerStats.ContainsKey(player1Id))
                playerStats[player1Id] = (allPlayers[player1Id], 0, 0);
            if (!playerStats.ContainsKey(player2Id))
                playerStats[player2Id] = (allPlayers[player2Id], 0, 0);

            if (match.WinnerId == player1Id)
            {
                playerStats[player1Id] = (playerStats[player1Id].Name, playerStats[player1Id].Wins + 1, playerStats[player1Id].Losses);
                playerStats[player2Id] = (playerStats[player2Id].Name, playerStats[player2Id].Wins, playerStats[player2Id].Losses + 1);
            }
            else
            {
                playerStats[player2Id] = (playerStats[player2Id].Name, playerStats[player2Id].Wins + 1, playerStats[player2Id].Losses);
                playerStats[player1Id] = (playerStats[player1Id].Name, playerStats[player1Id].Wins, playerStats[player1Id].Losses + 1);
            }
        }
    }
}
