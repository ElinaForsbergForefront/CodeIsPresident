using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using eWorldCup2.Domain.Models;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Domain.Validation;

namespace eWorldCup2.Application
{
    public class SpecifikPlayerRound
    {
        private readonly RoundRobinService _roundRobinService = new();

        public Result<(Player player, Player opponent)> GetSpecificRound(List<Player> players, int numberOfPlayers, int id, int specificRound)
        {
            var validateNumberOfPlayers = ValidatePlayers.ValidateNumberOfPlayers(numberOfPlayers);
            var idValidationResult = ValidationID.validateID(id, numberOfPlayers);
            var validationRoundResult = ValidationRounds.ValidateRound(numberOfPlayers, specificRound);

            if (!validateNumberOfPlayers.IsSuccess)
                return Result<(Player, Player)>.Failure(validateNumberOfPlayers.Error!);

            if (!validationRoundResult.IsSuccess)
                return Result<(Player, Player)>.Failure(validationRoundResult.Error!);
 
            if (!idValidationResult.IsSuccess)
                return Result<(Player, Player)>.Failure(idValidationResult.Error!);

            if (players.Count < numberOfPlayers)
                return Result<(Player, Player)>.Failure(new Error("INVALID_INPUT", "Spelarlistan har färre än n spelare."));


            var participatingPlayers = players.Take(numberOfPlayers).ToList();
            var selectedPlayer = participatingPlayers[id];

            var roundResult = _roundRobinService.GetRoundPairs(participatingPlayers, specificRound);
            if (!roundResult.IsSuccess)
                return Result<(Player, Player)>.Failure(roundResult.Error!);

            var roundPairs = roundResult.Value!;

            var playerMatch = roundPairs.FirstOrDefault(p =>
                ReferenceEquals(p.Item1, selectedPlayer) || ReferenceEquals(p.Item2, selectedPlayer));

            if (playerMatch == default)
                return Result<(Player, Player)>.Failure(new Error("NOT_FOUND", "Ingen match hittades för vald spelare."));

            var opponentPlayer = ReferenceEquals(playerMatch.Item1, selectedPlayer)
                ? playerMatch.Item2
                : playerMatch.Item1;


            return Result<(Player, Player)>.Success((selectedPlayer, opponentPlayer));
        }
    }
}
