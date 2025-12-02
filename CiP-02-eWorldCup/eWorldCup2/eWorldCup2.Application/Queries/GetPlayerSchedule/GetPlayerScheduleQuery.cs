using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.Models;

namespace eWorldCup2.Application.Queries.GetPlayerSchedule
{
    public record GetPlayerScheduleQuery(int NumberOfPlayers, int PlayerId);

    public record PlayerScheduleMatch(int Round, Player Player, Player Opponent);
    public record GetPlayerScheduleResult(List<PlayerScheduleMatch> Matches);

}
