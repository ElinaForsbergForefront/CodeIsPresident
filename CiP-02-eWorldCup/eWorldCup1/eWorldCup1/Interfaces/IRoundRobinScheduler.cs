using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Models;

namespace eWorldCup1.Interfaces
{
    public interface IRoundRobinScheduler
    {
        List<Match> GenerateMatches(List<Deltagare> deltagare, int round);
        Deltagare? FindOpponent(List<Deltagare> deltagre, int playerId, int round);
    }
}
