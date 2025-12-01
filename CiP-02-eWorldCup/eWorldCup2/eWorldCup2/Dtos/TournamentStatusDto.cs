namespace eWorldCup2.Api.Dtos
{
    public class TournamentStatusDto
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public int CurrentRound { get; set; }
        public int MaxRounds { get; set; }
        public PlayerInfoDto Player { get; set; } = null!;
        public PlayerInfoDto Opponent { get; set; } = null!;
        public MatchStatusDto Match { get; set; } = null!;
        public RoundStatusDto RoundStatus { get; set; } = null!;
    }

    public class PlayerInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class MatchStatusDto
    {
        public int MatchId { get; set; }
        public string Round { get; set; } = string.Empty;
        public int PlayerWins { get; set; }
        public int OpponentWins { get; set; }
        public int BestOf { get; set; }
        public bool IsCompleted { get; set; }
        public string? Winner { get; set; }
    }

    public class RoundStatusDto
    {
        public bool AllMatchesCompleted { get; set; }
        public bool OtherMatchesCompleted { get; set; }
        public int CompletedMatches { get; set; }
        public int TotalMatches { get; set; }
    }
}
