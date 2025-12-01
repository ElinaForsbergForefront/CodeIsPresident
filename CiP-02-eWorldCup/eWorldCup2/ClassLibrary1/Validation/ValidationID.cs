using eWorldCup2.Domain.ROP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWorldCup2.Domain.Validation
{
    public class ValidationID
    {
        public static Result<int> validateID(int id, int numberOfPlayers)
        {
            if (id < 0)
            {
                return Result<int>.Failure(new Error("INVALID_INPUT", "ID får inte vara mindre än 0"));
            }
            if (id >= numberOfPlayers)
            {
                return Result<int>.Failure(new Error("INVALID_INPUT", "ID får inte vara större än eller lika med antalet spelare."));
            }
            return Result<int>.Success(id);
        }
    }
}
