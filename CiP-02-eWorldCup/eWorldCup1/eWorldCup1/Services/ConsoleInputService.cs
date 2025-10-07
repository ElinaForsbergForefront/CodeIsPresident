using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Interfaces;
using eWorldCup1.Models;

namespace eWorldCup1.Services
{
    public class ConsoleInputService : IInputService
    {
        public TurneringstData GetTuneringsData()
        {
            Console.WriteLine("Tournament Scheduler");
            Console.WriteLine("==================");
            Console.Write("Number of deltagare (N): ");
            int n = int.Parse(Console.ReadLine()!);

            Console.Write("Round number (D): ");
            int d = int.Parse(Console.ReadLine()!);

            return new TurneringstData(n, d);
        }

        public int GetNumberOfPlayers()
        {
            Console.Write("Number of players: ");
            return int.Parse(Console.ReadLine()!);
        }

        public int GetRoundsPlayed()
        {
            Console.Write("Number of rounds played: ");
            return int.Parse(Console.ReadLine()!);
        }

        public (int playerId, int round) GetPlayerAndRound()
        {
            Console.Write("Player ID: ");
            int playerId = int.Parse(Console.ReadLine()!);

            Console.Write("Round number: ");
            int round = int.Parse(Console.ReadLine()!);

            return (playerId, round);
        }
    }
}
