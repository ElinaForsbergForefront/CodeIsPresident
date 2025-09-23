using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppHowManyFizzBuzz.Klasser
{
    public static class InputReader
    {
        public static readonly BigInteger MinValue = 1;
        public static readonly BigInteger MaxValue = BigInteger.Parse("1000000000000000"); // 10^15

        public static BigInteger ReadPositiveBigInteger(string prompt)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                var s = Console.ReadLine();
                if (BigInteger.TryParse(s, out var value) &&
                    value >= MinValue &&
                    value <= MaxValue)
                {
                    return value;
                }

                Console.WriteLine($"Fel. Ange ett heltal mellan {MinValue} och {MaxValue}:");
            }
        }
    }
}
