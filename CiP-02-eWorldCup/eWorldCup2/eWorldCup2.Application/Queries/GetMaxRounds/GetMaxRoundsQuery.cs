using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Queries.GetMaxRounds
{
    public record GetMaxRoundsQuery(int NumberOfPlayers);
    public record GetMaxRoundsResult(int MaxRounds);

}
