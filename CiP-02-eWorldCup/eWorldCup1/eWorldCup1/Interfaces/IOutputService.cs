using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Models;

namespace eWorldCup1.Interfaces
{
    public interface IOutputService
    {
        void DisplayMatches(List<Match> matches, int round);
        void DisplayTotalRounds(int totalRounds);
        void DisplayRemainingMatches(int remaining, int total);
        void DisplayOpponent(string playerName, string opponentName, int round);
        void DisplayError(string message);
    }
}
