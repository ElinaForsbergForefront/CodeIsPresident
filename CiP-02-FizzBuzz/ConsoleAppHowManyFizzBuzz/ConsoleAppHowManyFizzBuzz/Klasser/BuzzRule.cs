using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppHowManyFizzBuzz.Klasser
{
    public class BuzzRule: IFizzBuzzRule
    {
        public string Value => "Buzz";
        public BigInteger Calculation(BigInteger number)
        {
            return number / 5;
        }
    }
}
