using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWorldCup2.Domain.Validation
{
    public class ValidationRounds
    {
        public static Result<int> ValidateRound(int numberOfPlayers, int round)
        {
            if (round < 1)
            {
                return Result<int>.Failure(new Error("INVALID_INPUT", "Antalet rundor måste vara minst 1."));
            }
            if (round > numberOfPlayers - 1)
            {
                return Result<int>.Failure(new Error("INVALID_INPUT", "Antalet rundor kan inte vara större än maximala rundor."));

            }
            return Result<int>.Success(round);
        }
    }
}
