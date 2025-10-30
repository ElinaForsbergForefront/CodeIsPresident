using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eWorldCup2.Domain
{
    public readonly record struct Error(string Code, string Message)
    {
        public static Error None => new("", "");
        public override string ToString() => $"{Code}: {Message}";
    }
}
