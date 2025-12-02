using eWorldCup2.Api.Dtos;
using eWorldCup2.Application.Commands.CreatePlayer;
using eWorldCup2.Application.Commands.DeletePlayer;
using eWorldCup2.Application.Queries.GetAllPlayers;
using eWorldCup2.Application.Queries.GetRoundPairs;
using eWorldCup2.Application.Queries.GetMaxRounds;
using eWorldCup2.Application.Queries.GetRemainingPairs;
using eWorldCup2.Application.Queries.GetPlayerMatch;
using eWorldCup2.Application.Queries.GetPlayerSchedule;
using eWorldCup2.Application.Queries.GetPlayerMatchByName;
using Microsoft.AspNetCore.Mvc;

namespace eWorldCup2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundRobinController : ControllerBase
    {
        private readonly CreatePlayerCommandHandler createPlayerHandler;
        private readonly DeletePlayerCommandHandler deletePlayerHandler;
        private readonly GetAllPlayersQueryHandler getAllPlayersHandler;
        private readonly GetRoundPairsQueryHandler getRoundPairsHandler;
        private readonly GetMaxRoundsQueryHandler getMaxRoundsHandler;
        private readonly GetRemainingPairsQueryHandler getRemainingPairsHandler;
        private readonly GetPlayerMatchQueryHandler getPlayerMatchHandler;
        private readonly GetPlayerScheduleQueryHandler getPlayerScheduleHandler;
        private readonly GetPlayerMatchByNameQueryHandler getPlayerMatchByNameHandler;

        public RoundRobinController(
            CreatePlayerCommandHandler createPlayerHandler,
            DeletePlayerCommandHandler deletePlayerHandler,
            GetAllPlayersQueryHandler getAllPlayersHandler,
            GetRoundPairsQueryHandler getRoundPairsHandler,
            GetMaxRoundsQueryHandler getMaxRoundsHandler,
            GetRemainingPairsQueryHandler getRemainingPairsHandler,
            GetPlayerMatchQueryHandler getPlayerMatchHandler,
            GetPlayerScheduleQueryHandler getPlayerScheduleHandler,
            GetPlayerMatchByNameQueryHandler getPlayerMatchByNameHandler)
        {
            this.createPlayerHandler = createPlayerHandler;
            this.deletePlayerHandler = deletePlayerHandler;
            this.getAllPlayersHandler = getAllPlayersHandler;
            this.getRoundPairsHandler = getRoundPairsHandler;
            this.getMaxRoundsHandler = getMaxRoundsHandler;
            this.getRemainingPairsHandler = getRemainingPairsHandler;
            this.getPlayerMatchHandler = getPlayerMatchHandler;
            this.getPlayerScheduleHandler = getPlayerScheduleHandler;
            this.getPlayerMatchByNameHandler = getPlayerMatchByNameHandler;
        }

        [HttpGet("players")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var query = new GetAllPlayersQuery();
            var result = await getAllPlayersHandler.HandleAsync(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Error!.Message });
            }

            return Ok(result.Value!.Players);
        }

        [HttpPost("playersCreate")]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerDto dto)
        {
            var command = new CreatePlayerCommand(dto.Name);
            var result = await createPlayerHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "INVALID_NAME" => BadRequest(new { Error = result.Error.Message }),
                    "PLAYER_EXISTS" => BadRequest(new { Error = result.Error.Message }),
                    _ => BadRequest(new { Error = result.Error.Message })
                };
            }

            var player = result.Value!.Player;
            return CreatedAtAction(nameof(GetAllPlayers), new { id = player.Id }, player);
        }

        [HttpDelete("playersDelete/{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var command = new DeletePlayerCommand(id);
            var result = await deletePlayerHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "PLAYER_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    _ => BadRequest(new { message = result.Error.Message })
                };
            }

            return NoContent();
        }

        [HttpGet("rounds/{specificRound}")]
        public async Task<IActionResult> GetPlayersSpecificRound(int specificRound)
        {
            var query = new GetRoundPairsQuery(specificRound);
            var result = await getRoundPairsHandler.HandleAsync(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value!.MatchPairs);
        }

        [HttpGet("rounds/max/{numberOfPlayers}")]
        public IActionResult GetMaxRounds(int numberOfPlayers)
        {
            var query = new GetMaxRoundsQuery(numberOfPlayers);
            var result = getMaxRoundsHandler.Handle(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value!.MaxRounds);
        }

        [HttpGet("match/remaining")]
        public IActionResult GetUniquePairsAfterXRounds(int numberOfPlayers, int completedRounds)
        {
            var query = new GetRemainingPairsQuery(numberOfPlayers, completedRounds);
            var result = getRemainingPairsHandler.Handle(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value!.RemainingPairs);
        }

        [HttpGet("rounds/{id}/{specificRound}")]
        public async Task<IActionResult> GetSpecificRoundMatches(int numberOfPlayers, int id, int specificRound)
        {
            var query = new GetPlayerMatchQuery(numberOfPlayers, id, specificRound);
            var result = await getPlayerMatchHandler.HandleAsync(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            var data = result.Value!;
            return Ok(new { Player = data.Player, Opponent = data.Opponent });
        }

        [HttpGet("player/{id}/schedule/{numberOfPlayers}")]
        public async Task<IActionResult> GetAllMatchesForPlayer(int numberOfPlayers, int id)
        {
            var query = new GetPlayerScheduleQuery(numberOfPlayers, id);
            var result = await getPlayerScheduleHandler.HandleAsync(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            var matches = result.Value!.Matches.Select(m => new
            {
                Round = m.Round,
                Player = m.Player,
                Opponent = m.Opponent
            });

            return Ok(matches);
        }

        [HttpGet("name/rounds/{name}/{specificRound}")]
        public async Task<IActionResult> GetSpecificRoundMatchesByName(string name, int specificRound)
        {
            var query = new GetPlayerMatchByNameQuery(name, specificRound);
            var result = await getPlayerMatchByNameHandler.HandleAsync(query);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "PLAYER_NOT_FOUND" => NotFound(new { Error = result.Error.Message }),
                    _ => BadRequest(result.Error)
                };
            }

            var data = result.Value!;
            return Ok(new { Player = data.Player, Opponent = data.Opponent });
        }
    }
}