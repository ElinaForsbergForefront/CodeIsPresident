using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup2.Domain;
using eWorldCup2.Domain.Validation;

namespace eWorldCup2.Application
{
    public class RemainingPairsCalculator
    {
        public Result<int> CalculateRemainingPairs(int numberOfPlayers, int completedRounds)
        {
            int maxRounds = numberOfPlayers - 1;

            var validationResult = ValidatePlayers.ValidateNumberOfPlayers(numberOfPlayers);
            if (!validationResult.IsSuccess)
                return Result<int>.Failure(validationResult.Error!);

            var validateCompletedRoundsResult = ValidateCompletedRounds.ValidateCompletedRound(completedRounds, maxRounds);
            if (!validateCompletedRoundsResult.IsSuccess)
                return Result<int>.Failure(validateCompletedRoundsResult.Error!);

            int remainingRounds = maxRounds - completedRounds;
            int pairsPerRound = numberOfPlayers / 2;
            int remainingPairs = remainingRounds * pairsPerRound;

            return Result<int>.Success(remainingPairs);
        }
    }
}
