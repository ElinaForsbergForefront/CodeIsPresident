using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup2.Domain.ROP;
using eWorldCup2.Domain.Validation;

namespace eWorldCup2.Application
{
    public class MaxRoundsCalculator
    {
        public Result<int> CalculateMaxRounds(int numberOfPlayers)
        {
            var validationResult =  ValidatePlayers.ValidateNumberOfPlayers(numberOfPlayers);

            if (!validationResult.IsSuccess)
                return Result<int>.Failure(validationResult.Error!);
            
            return Result<int>.Success(numberOfPlayers - 1);
        }
    }
}
