using eWorldCup2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Commands.StartTournament
{
    public record StartTournamentCommand(string Name, int NumberOfPlayers);

    public record StartTournamentResult(
        Tournament Tournament,
        List<Match> Matches,
        List<Player> Players
    );
}
