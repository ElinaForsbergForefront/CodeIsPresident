using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWorldCup1.Models
{
    public class Match
    {
        public Deltagare Player1 { get; }
        public Deltagare Player2 { get; }

        public Match(Deltagare player1, Deltagare player2)
        {
            Player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
            Player2 = player2 ?? throw new ArgumentNullException(nameof(player2));
        }

        public override string ToString() => $"{Player1.Name} vs {Player2.Name}";
    }
}
