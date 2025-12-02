using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.Models;

namespace eWorldCup2.Application.Queries.GetAllPlayers
{
    public record GetAllPlayersQuery();
    public record GetAllPlayersResult(List<Player> Players);
}
