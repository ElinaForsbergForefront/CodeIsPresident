using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Domain.Models
{
    public sealed record Tournament
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int NumberOfPlayers { get; init; }
        public int CurrentRound { get; init; }
        public DateTime StartedAt { get; init; }
        public bool IsCompleted { get; init; }

        public ICollection<Match> Matches { get; init; } = new List<Match>();
    }
}
