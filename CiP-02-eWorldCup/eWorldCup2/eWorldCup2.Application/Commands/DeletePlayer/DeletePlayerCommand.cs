using System;
using System.Collections.Generic;
using System.Text;

namespace eWorldCup2.Application.Commands.DeletePlayer
{
    public record DeletePlayerCommand(int PlayerId);
    public record DeletePlayerResult(bool Success);
}
