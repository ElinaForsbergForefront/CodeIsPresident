using eWorldCup2.Domain.Models;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Commands.StartTournament
{
    public class StartTournamentCommandHandler
    {
        private readonly WorldCupDbContext dbContext;
        private readonly RoundRobinService roundRobinService;

        public StartTournamentCommandHandler(
            WorldCupDbContext dbContext,
            RoundRobinService roundRobinService)
        {
            this.dbContext = dbContext;
            this.roundRobinService = roundRobinService;
        }

        public async Task<Result<StartTournamentResult>> HandleAsync(StartTournamentCommand command)
        {
            // Validate and get players
            var playersResult = await GetPlayersAsync(command.NumberOfPlayers);
            if (!playersResult.IsSuccess)
            {
                return Result<StartTournamentResult>.Failure(playersResult.Error!);
            }
            var players = playersResult.Value!;

            // Create tournament
            var tournament = new Tournament
            {
                Name = command.Name,
                NumberOfPlayers = command.NumberOfPlayers,
                CurrentRound = 1,
                StartedAt = DateTime.UtcNow,
                IsCompleted = false
            };

            dbContext.Tournaments.Add(tournament);
            await dbContext.SaveChangesAsync();

            // Create matches
            var pairingResult = roundRobinService.GetRoundPairs(players, 1);
            if (!pairingResult.IsSuccess)
            {
                return Result<StartTournamentResult>.Failure(pairingResult.Error!);
            }

            var matches = pairingResult.Value!.Select(pair => new Match
            {
                TournamentId = tournament.Id,
                Round = 1,
                Player1Id = pair.Item1.Id,
                Player2Id = pair.Item2.Id,
                Player1Wins = 0,
                Player2Wins = 0,
                IsCompleted = false,
                WinnerId = null
            }).ToList();

            dbContext.Matches.AddRange(matches);
            await dbContext.SaveChangesAsync();

            var result = new StartTournamentResult(tournament, matches, players);
            return Result<StartTournamentResult>.Success(result);
        }

        private async Task<Result<List<Player>>> GetPlayersAsync(int numberOfPlayers)
        {
            var players = await dbContext.Players
                .Take(numberOfPlayers)
                .ToListAsync();

            if (players.Count < numberOfPlayers)
            {
                return Result<List<Player>>.Failure(
                    new Error("INSUFFICIENT_PLAYERS",
                    $"Not enough players in database. Requested: {numberOfPlayers}, Available: {players.Count}"));
            }

            return Result<List<Player>>.Success(players);
        }
    }
}
