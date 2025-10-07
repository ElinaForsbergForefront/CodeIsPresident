using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Interfaces;
using eWorldCup1.Models;

namespace eWorldCup1.Services
{
    public class TournamentService
    {
        private readonly IDeltagareRepository _delatagareRepository;
        private readonly IRoundRobinScheduler _scheduler;
        private readonly IInputService _inputService;
        private readonly IOutputService _outputService;

        public TournamentService(
            IDeltagareRepository deltagareRepository,
            IRoundRobinScheduler scheduler,
            IInputService inputService,
            IOutputService outputService)
        {
            _delatagareRepository = deltagareRepository;
            _scheduler = scheduler;
            _inputService = inputService;
            _outputService = outputService;
        }

        public void RunTurnering()
        {
            try
            {
                var deltagare = _delatagareRepository.GetDeltagare();
                var turneringstData = _inputService.GetTuneringsData();

                ValidateParticipantCount(deltagare.Count, turneringstData.NumberOfDeltagare);

                var matches = _scheduler.GenerateMatches(deltagare, turneringstData.SpecificRound);
                _outputService.DisplayMatches(matches, turneringstData.SpecificRound);

                RunAdditionalFeatures(deltagare);
            }
            catch (Exception ex)
            {
                _outputService.DisplayError(ex.Message);
            }
        }

        private void RunAdditionalFeatures(List<Deltagare> deltagare)
        {
            CalculateTotalRounds();
            CalculateRemainingMatches();
            FindSpecificOpponent(deltagare);
        }

        private void CalculateTotalRounds()
        {
            Console.WriteLine("\n=== Part 2.1 ===");
            int totalPlayers = _inputService.GetNumberOfPlayers();
            int totalRounds = totalPlayers - 1;
            _outputService.DisplayTotalRounds(totalRounds);
        }

        private void CalculateRemainingMatches()
        {
            Console.WriteLine("\n=== Part 2.2 ===");
            int totalPlayers = _inputService.GetNumberOfPlayers();
            int roundsPlayed = _inputService.GetRoundsPlayed();

            int totalMatches = (totalPlayers * (totalPlayers - 1)) / 2;
            int matchesPlayed = (roundsPlayed * totalPlayers) / 2;
            int remainingMatches = totalMatches - matchesPlayed;

            _outputService.DisplayRemainingMatches(remainingMatches, totalMatches);
        }

        private void FindSpecificOpponent(List<Deltagare> deltagare)
        {
            Console.WriteLine("\n=== Part 2.3 ===");
            var (playerId, round) = _inputService.GetPlayerAndRound();

            var player = deltagare.Find(p => p.Id == playerId);
            var opponent = _scheduler.FindOpponent(deltagare, playerId, round);

            if (player != null && opponent != null)
            {
                _outputService.DisplayOpponent(player.Name, opponent.Name, round);
            }
            else
            {
                _outputService.DisplayError("Player or opponent not found");
            }
        }

        private static void ValidateParticipantCount(int available, int requested)
        {
            if (requested > available)
            {
                throw new InvalidOperationException($"Only {available} participants available, but {requested} requested.");
            }
        }
    }
}
