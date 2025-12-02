using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Queries.GetRemainingPairs
{
    public record GetRemainingPairsQuery(int NumberOfPlayers, int CompletedRounds);
    public record GetRemainingPairsResult(int RemainingPairs);
}
