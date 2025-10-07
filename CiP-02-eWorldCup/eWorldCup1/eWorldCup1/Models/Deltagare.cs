using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eWorldCup1.Models
{
    public class Deltagare
    {
        public string Name { get; }
        public int Id { get; }

        public Deltagare(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
