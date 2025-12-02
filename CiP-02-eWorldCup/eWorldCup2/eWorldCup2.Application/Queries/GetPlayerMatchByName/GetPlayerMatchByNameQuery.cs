using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.Models;

namespace eWorldCup2.Application.Queries.GetPlayerMatchByName
{
    public record GetPlayerMatchByNameQuery(string PlayerName, int SpecificRound);
    public record GetPlayerMatchByNameResult(Player Player, Player Opponent);

}
