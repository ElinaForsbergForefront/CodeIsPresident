using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ConsoleAppHowManyFizzBuzz.Klasser
{
    public class FizzRule : IFizzBuzzRule
    {
        public string Value => "Fizz";
        public BigInteger Calculation(BigInteger number)
        {
            return number / 3;
        }
    }
}
