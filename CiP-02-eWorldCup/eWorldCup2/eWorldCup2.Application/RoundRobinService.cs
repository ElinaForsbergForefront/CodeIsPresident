using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using eWorldCup2.Domain;
using eWorldCup2.Domain.Validation;

namespace eWorldCup2.Application
{
    public class RoundRobinService   
    {
        public Result<List<(Player, Player)>> GetRoundPairs(List<Player> players, int roundNumber)
        {

            int numberOfPlayers = players.Count;
          
            var validationResult = ValidatePlayers.ValidateNumberOfPlayers(numberOfPlayers);
            var validateRoundResult = ValidationRounds.ValidateRound(numberOfPlayers, roundNumber);

            if (!validationResult.IsSuccess)
                return Result<List<(Player, Player)>>.Failure(validationResult.Error!);

            if (!validateRoundResult.IsSuccess)
                return Result<List<(Player, Player)>>.Failure(validateRoundResult.Error!);


            int fixedPlayerIndex = 0; 
            int zeroBasedRoundIndex = roundNumber - 1; 

            var rotatingIndices = Enumerable.Range(1, numberOfPlayers - 1).ToList();

            RotateRight(rotatingIndices, zeroBasedRoundIndex);

            var roundPairs = new List<(Player, Player)>(numberOfPlayers / 2)
            {
                (players[fixedPlayerIndex], players[rotatingIndices[0]])
            };

            for (int i = 1; i <= rotatingIndices.Count / 2; i++)
            {
                int leftPlayerIndex = rotatingIndices[i];
                int rightPlayerIndex = rotatingIndices[rotatingIndices.Count - i];
                roundPairs.Add((players[leftPlayerIndex], players[rightPlayerIndex]));
            }

            return Result<List<(Player, Player)>>.Success(roundPairs);
        }

        private static void RotateRight(List<int> list, int steps)
        {
            if (list.Count == 0) return;
            steps %= list.Count;
            if (steps == 0) return;

            var movedItems = list.GetRange(list.Count - steps, steps);
            list.RemoveRange(list.Count - steps, steps);
            list.InsertRange(0, movedItems);
        }
    }
}
