using eWorldCup2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Commands.AdvanceRound
{
    public record AdvanceRoundCommand(int TournamentId);

    public record AdvanceRoundResult(
        Tournament Tournament,
        List<Match>? NewMatches,
        List<Player>? Players,
        bool IsCompleted
    );
}
