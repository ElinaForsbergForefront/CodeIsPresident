using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eWorldCup1.Interfaces;
using eWorldCup1.Models;

namespace eWorldCup1.Repositories
{
    public class DeltagareRepository : IDeltagareRepository
    {
        public List<Deltagare> GetDeltagare()
        {
            return new List<Deltagare>
            {
                new(1, "Kalle"),
                new(2, "Lisa"),
                new(3, "Olle"),
                new(4, "Anna"),
                new(5, "Eva"),
                new(6, "Mats"),
                new(7, "Sara"),
                new(8, "Per")
            };
        }
    }
}
