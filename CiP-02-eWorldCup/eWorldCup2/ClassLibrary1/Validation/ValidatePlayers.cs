using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup2.Domain.ROP;

namespace eWorldCup2.Domain.Validation
{
    public class ValidatePlayers
    {
        public static Result<int> ValidateNumberOfPlayers(int numberOfPlayers)
        {
            if (numberOfPlayers < 2)
            {
                return Result<int>.Failure(new Error("INVALID_INPUT", "Antalet deltagare måste vara minst 2."));
            }
            if (numberOfPlayers % 2 != 0)
            {
                return Result<int>.Failure(new Error("INVALID_INPUT", "Antalet deltagare måste vara jämnt."));
            }


            return Result<int>.Success(numberOfPlayers);
        }
    }
}
