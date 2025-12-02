using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.Models;

namespace eWorldCup2.Application.Queries.GetPlayerMatch
{
    public record GetPlayerMatchQuery(int NumberOfPlayers, int PlayerId, int SpecificRound);

    public record GetPlayerMatchResult(Player Player, Player Opponent);

}
