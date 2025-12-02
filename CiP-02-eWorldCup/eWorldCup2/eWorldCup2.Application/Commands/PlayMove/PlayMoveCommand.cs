using eWorldCup2.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Commands.PlayMove
{
    public record PlayMoveCommand(
        int TournamentId,
        int PlayerId,
        string Move
    );

    public record PlayMoveResult(
        Match UpdatedMatch,
        Player Player,
        Player Opponent,
        bool IsPlayer1,
        int PlayerMove,
        int OpponentMove,
        int GameResult
    );
}
