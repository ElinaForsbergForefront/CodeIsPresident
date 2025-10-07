using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Interfaces;
using eWorldCup1.Models;

namespace eWorldCup1.Services
{
    public class ConsoleOutputService : IOutputService
    {
        public void DisplayMatches(List<Match> matches, int round)
        {
            Console.WriteLine($"\nRound {round}:");
            Console.WriteLine("Matches:");
            foreach (var match in matches)
            {
                Console.WriteLine(match);
            }
        }

        public void DisplayTotalRounds(int totalRounds)
        {
            Console.WriteLine($"Total rounds to be played: {totalRounds}");
        }

        public void DisplayRemainingMatches(int remaining, int total)
        {
            Console.WriteLine($"Total unique pairs that can be formed: {total}");
            Console.WriteLine($"Remaining unique pairs to meet: {remaining}");
        }

        public void DisplayOpponent(string playerName, string opponentName, int round)
        {
            Console.WriteLine($"\nPlayer {playerName} meets {opponentName} in round {round}.");
        }

        public void DisplayError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }
    }
}
