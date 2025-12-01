using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Domain.Models
{
    public sealed record Match
    {
        public int Id { get; init; }
        public int TournamentId { get; init; }
        public int Round { get; init; }
        public int Player1Id { get; init; }
        public int Player2Id { get; init; }
        public int Player1Wins { get; init; }
        public int Player2Wins { get; init; }
        public bool IsCompleted { get; init; }
        public int? WinnerId { get; init; }
    }
}
