using System.Numerics;
using eWorldCup2.Application;
using eWorldCup2.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eWorldCup2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundRobinController : ControllerBase
    {
        [HttpGet("players")]
        public IActionResult GetAllPlayers()
        {
            var players = new List<Player>
            {
                new(1, "Alice"),
                new(2, "Bob"),
                new(3, "Charlie"),
                new(4, "Diana"),
                new(5, "Ethan"),
                new(6, "Fiona"),
                new(7, "George"),
                new(8, "Hannah"),
                new(9, "Isaac"),
                new(10, "Julia"),
                new(11, "Kevin"),
                new(12, "Laura"),
                new(13, "Michael"),
                new(14, "Nina"),
                new(15, "Oscar"),
                new(16, "Paula"),
                new(17, "Quentin"),
                new(18, "Rachel"),
                new(19, "Samuel"),
                new(20, "Tina")
            };

            return Ok(players);
        }

        [HttpGet("/rounds/{specificRound}")]
        //Return all round robin matches in a specific round
        public IActionResult GetPlayersSpecificRound(int specificRound)
        {
            var players = new List<Player>
            {
                new(1, "Alice"),
                new(2, "Bob"),
                new(3, "Charlie"),
                new(4, "Diana"),
                new(5, "Ethan"),
                new(6, "Fiona"),
                new(7, "George"),
                new(8, "Hannah"),
                new(9, "Isaac"),
                new(10, "Julia"),
            };

            var roundRobinService = new RoundRobinService();
            var result = roundRobinService.GetRoundPairs(players, specificRound); 

            if (result.IsSuccess)
            {
                var lines = new List<string>();
                foreach (var (a, b) in result.Value!)
                    lines.Add($"{a.Name} vs {b.Name}");
                return Ok(lines);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpGet("/rounds/max/{numberOfPlayers}")]
        //RETURN max rounds for n players
        public IActionResult GetMaxRounds(int numberOfPlayers)
        {
            var maxRounds = new MaxRoundsCalculator();
            var result = maxRounds.CalculateMaxRounds(numberOfPlayers);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpGet("/match/remaining")]

        public IActionResult GetUniquePairsAfterXRounds(int numberOfPlayers, int completedRounds)
        {
            var remainingPairs = new RemainingPairsCalculator();

            var result = remainingPairs.CalculateRemainingPairs(numberOfPlayers, completedRounds);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpGet("rounds/{id}/{specificRound}")]

        //Return specific round match for a specific player
        public IActionResult GetSpecificRoundMatches(int numberOfPlayers, int id, int specificRound)
        {
            var players = new List<Player>
            {
                new(1, "Alice"),
                new(2, "Bob"),
                new(3, "Charlie"),
                new(4, "Diana"),
                new(5, "Ethan"),
                new(6, "Fiona"),
                new(7, "George"),
                new(8, "Hannah"),
                new(9, "Isaac"),
                new(10, "Julia"),
                new(11, "Kevin"),
                new(12, "Laura"),
                new(13, "Michael"),
                new(14, "Nina"),
                new(15, "Oscar"),
                new(16, "Paula"),
                new(17, "Quentin"),
                new(18, "Rachel"),
                new(19, "Samuel"),
                new(20, "Tina")
            };


            var round = new SpecifikPlayerRound();
            var result = round.GetSpecificRound(players, numberOfPlayers, id, specificRound);

            if (result.IsSuccess)
            {
                var (player, opponent) = result.Value;
                return Ok(new { Player = player, Opponent = opponent });
            }
            else
            {
                return BadRequest(result.Error);
            }

        }

        [HttpGet("player/{id}/schedule/{numberOfPlayers}")]

        //Return all matches for a specific player
        public IActionResult GetAllMatchesForPlayer(int numberOfPlayers, int id)
        {
            var players = new List<Player>
            {
                new(1, "Alice"),
                new(2, "Bob"),
                new(3, "Charlie"),
                new(4, "Diana"),
                new(5, "Ethan"),
                new(6, "Fiona"),
                new(7, "George"),
                new(8, "Hannah"),
                new(9, "Isaac"),
                new(10, "Julia"),
                new(11, "Kevin"),
                new(12, "Laura"),
                new(13, "Michael"),
                new(14, "Nina"),
                new(15, "Oscar"),
                new(16, "Paula"),
                new(17, "Quentin"),
                new(18, "Rachel"),
                new(19, "Samuel"),
                new(20, "Tina")
            };

            var round = new SpecifikPlayerRound();
            var matches = new List<object>();

            for (int specificRound = 1; specificRound < numberOfPlayers; specificRound++)
            {
                var result = round.GetSpecificRound(players, numberOfPlayers, id, specificRound);

                if (result.IsSuccess)
                {
                    var (player, opponent) = result.Value;
                    matches.Add(new
                    {
                        Round = specificRound,
                        Player = player,
                        Opponent = opponent
                    });
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }

            return Ok(matches);
        }

        [HttpGet("name/rounds/{name}/{specificRound}")]
        //Return specific round match for a specific player
        //Alias till “direktfråga” för spelare i i runda d, men med namn/objekt.
        public IActionResult GetSpecificRoundMatchesByName(string name, int specificRound)
        {
            var players = new List<Player>
            {
                new(1, "Alice"),
                new(2, "Bob"),
                new(3, "Charlie"),
                new(4, "Diana"),
                new(5, "Ethan"),
                new(6, "Fiona"),
                new(7, "George"),
                new(8, "Hannah"),
                new(9, "Isaac"),
                new(10, "Julia"),
                new(11, "Kevin"),
                new(12, "Laura"),
                new(13, "Michael"),
                new(14, "Nina"),
                new(15, "Oscar"),
                new(16, "Paula"),
                new(17, "Quentin"),
                new(18, "Rachel"),
                new(19, "Samuel"),
                new(20, "Tina")
            };

            var round = new SpecifikPlayerRound();

            var getId = players.Find(p => p.Name == name);

            if (getId is null)
            {
                return NotFound(new { Error = $"Spelare med namn '{name}' hittades inte." });
            }



            var result = round.GetSpecificRound(players, players.Count, getId.Id-1, specificRound);
            if (result.IsSuccess)
            {
                var (player, opponent) = result.Value;
                return Ok(new { Player = player, Opponent = opponent });
            }
            else
            {
                return BadRequest(result.Error);
            }

        }
    }
}