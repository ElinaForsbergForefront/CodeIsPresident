using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Models;

namespace eWorldCup1.Interfaces
{
    public interface IDeltagareRepository
    {
        List<Deltagare> GetDeltagare();
    }
}
