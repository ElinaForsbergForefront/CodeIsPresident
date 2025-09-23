using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppHowManyFizzBuzz.Klasser
{
    public interface IFizzBuzzRule
    {
        string Value { get; }
        BigInteger Calculation(BigInteger number);
    }
}
