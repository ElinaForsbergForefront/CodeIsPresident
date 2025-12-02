using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Queries.GetRoundPairs
{
    public record GetRoundPairsQuery(int SpecificRound);

    public record GetRoundPairsResult(List<string> MatchPairs);
}
