using eWorldCup2.Domain.ROP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWorldCup2.Domain.Validation
{
    public class ValidateCompletedRounds
    {

        public static Result<int> ValidateCompletedRound(int completedRounds, int maxRounds)
        {
            if (completedRounds < 0)
            {
                return Result<int>.Failure(new Error("INVALID_INPUT", "Genomförda rundor får inte vara mindre än 0"));
            }

            if(completedRounds > maxRounds) {
                return Result<int>.Failure(new Error("INVALID_INPUT", "Antalet genomförda rundor kan inte vara större än maximala rundor."));
            }

            return Result<int>.Success(completedRounds);
        }
    }
}
