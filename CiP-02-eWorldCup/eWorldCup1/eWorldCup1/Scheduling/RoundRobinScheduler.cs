using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Models;
using eWorldCup1.Interfaces;

namespace eWorldCup1.Scheduling
{
    public class RoundRobinScheduler : IRoundRobinScheduler
    {
        public List<Match> GenerateMatches(List<Deltagare> deltagare, int round)
        {
            var matches = new List<Match>();
            int n = deltagare.Count;
            int roundIndex = round - 1;

            for (int i = 0; i < n / 2; i++)
            {
                var (player1Index, player2Index) = CalculatePlayerIndices(i, roundIndex, n);
                matches.Add(new Match(deltagare[player1Index], deltagare[player2Index]));
            }

            return matches;
        }

        public Deltagare? FindOpponent(List<Deltagare> deltagare, int playerId, int round)
        {
            var matches = GenerateMatches(deltagare, round);

            foreach (var match in matches)
            {
                if (match.Player1.Id == playerId)
                    return match.Player2;
                if (match.Player2.Id == playerId)
                    return match.Player1;
            }

            return null;
        }

        private static (int player1Index, int player2Index) CalculatePlayerIndices(int matchIndex, int roundIndex, int totalPlayers)
        {
            int player1Index, player2Index;

            if (matchIndex == 0)
            {
                player1Index = 0;
                player2Index = totalPlayers - 1 - roundIndex;
                if (player2Index <= 0)
                    player2Index = totalPlayers - 1;
            }
            else
            {
                int leftIndex = (roundIndex + matchIndex) % (totalPlayers - 1);
                int rightIndex = (roundIndex - matchIndex + totalPlayers - 1) % (totalPlayers - 1);

                player1Index = leftIndex == 0 ? totalPlayers - 1 : leftIndex;
                player2Index = rightIndex == 0 ? totalPlayers - 1 : rightIndex;
            }

            return (player1Index, player2Index);
        }
    }
}
