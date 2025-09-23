using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ConsoleAppHowManyFizzBuzz.Klasser
{
    public class FizzBuzzCalculator
    {
        private readonly List<IFizzBuzzRule> _rules;
        public FizzBuzzCalculator(List<IFizzBuzzRule> rules)
        {
            _rules = rules;
        }
        public BigInteger Calculate(BigInteger number)
        {
            BigInteger result = 0;
            foreach (var rule in _rules)
            {
                result += rule.Calculation(number);
            }
            return result;
        }
    }
}
