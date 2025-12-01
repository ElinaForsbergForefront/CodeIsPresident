namespace eWorldCup2.Api.Dtos
{
    public class PlayMoveDto
    {
        public int TournamentId { get; set; }
        public int PlayerId { get; set; }
        public string Move { get; set; } = string.Empty; // "rock", "paper", or "scissors"
    }
}