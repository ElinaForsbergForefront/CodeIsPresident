using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWorldCup1.Models
{
    public class TurneringstData
    {
        public int NumberOfDeltagare { get; }
        public int SpecificRound { get; }

        public TurneringstData(int numberOfDelatager, int specificRound)
        {
            if (numberOfDelatager <= 0 || numberOfDelatager % 2 != 0)
                throw new ArgumentException("Number of participants must be positive and even");

            if (specificRound < 1 || specificRound > numberOfDelatager - 1)
                throw new ArgumentException($"Round must be between 1 and {numberOfDelatager - 1}");

            NumberOfDeltagare = numberOfDelatager;
            SpecificRound = specificRound;
        }
    }
}
