using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.Models;

namespace eWorldCup2.Application.Commands.CreatePlayer
{
    public record CreatePlayerCommand(string Name);

    public record CreatePlayerResult(Player Player);
}
