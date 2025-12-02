using eWorldCup2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Queries.GetFinalResults
{
    public record GetFinalResultsQuery(int TournamentId);

    public record FinalResultsResult(
        Tournament Tournament,
        List<ScoreboardEntry> Scoreboard
    );

    public record ScoreboardEntry(
        int Rank,
        int PlayerId,
        string PlayerName,
        int Wins,
        int Losses
    );
}
