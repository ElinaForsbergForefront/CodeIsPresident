using eWorldCup2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Queries.GetTournamentStatus
{
    public record GetTournamentStatusQuery(int TournamentId, int PlayerId);

    public record TournamentStatusResult(
        Tournament Tournament,
        Match CurrentMatch,
        Player Player,
        Player Opponent,
        bool IsPlayer1,
        List<Match> AllMatchesInRound
    );
}
