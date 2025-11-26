using System.Numerics;
using System.Runtime.CompilerServices;
using eWorldCup2.Api.Dtos;
using eWorldCup2.Application;
using eWorldCup2.Domain.Models;
using eWorldCup2.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eWorldCup2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundRobinController : ControllerBase
    {
        private readonly WorldCupDbContext dbContext;

        public RoundRobinController(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("players")]
        public IActionResult GetAllPlayers()
        {
            var players = dbContext.Players.ToList();

            return Ok(players);
        }

        [HttpPost("playersCreate")]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new { Error = "Spelarnamn får inte vara tomt." });
            }

            if (await dbContext.Players.AnyAsync(p => p.Name == dto.Name))
            {
                return BadRequest(new { Error = $"Spelare '{dto.Name}' finns redan." });
            }

            var player = new Player(0, dto.Name);
            dbContext.Players.Add(player);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllPlayers), new { id = player.Id }, player);
        }

        [HttpDelete("playersDelete/{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await dbContext.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            dbContext.Players.Remove(player);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("rounds/{specificRound}")]
        //Return all round robin matches in a specific round
        public IActionResult GetPlayersSpecificRound(int specificRound)
        {
            var players = dbContext.Players.ToList();

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

        [HttpGet("rounds/max/{numberOfPlayers}")]
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

        [HttpGet("match/remaining")]

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
            var players = dbContext.Players.ToList();


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
            var players = dbContext.Players.ToList();

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
            var players = dbContext.Players.ToList();
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