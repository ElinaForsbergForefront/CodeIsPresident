
using eWorldCup2.Api.Dtos;
using eWorldCup2.Application.Commands.AdvanceRound;
using eWorldCup2.Application.Commands.PlayMove;
using eWorldCup2.Application.Commands.StartTournament;
using eWorldCup2.Application.Queries.GetFinalResults;
using eWorldCup2.Application.Queries.GetTournamentStatus;
using Microsoft.AspNetCore.Mvc;

namespace eWorldCup2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly StartTournamentCommandHandler startTournamentHandler;
        private readonly PlayMoveCommandHandler playMoveHandler;
        private readonly AdvanceRoundCommandHandler advanceRoundHandler;
        private readonly GetTournamentStatusQueryHandler getTournamentStatusHandler;
        private readonly GetFinalResultsQueryHandler getFinalResultsHandler;

        public TournamentController(
            StartTournamentCommandHandler startTournamentHandler,
            PlayMoveCommandHandler playMoveHandler,
            AdvanceRoundCommandHandler advanceRoundHandler,
            GetTournamentStatusQueryHandler getTournamentStatusHandler,
            GetFinalResultsQueryHandler getFinalResultsHandler)
        {
            this.startTournamentHandler = startTournamentHandler;
            this.playMoveHandler = playMoveHandler;
            this.advanceRoundHandler = advanceRoundHandler;
            this.getTournamentStatusHandler = getTournamentStatusHandler;
            this.getFinalResultsHandler = getFinalResultsHandler;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartTournament([FromBody] StartTournamentDto request)
        {
            var command = new StartTournamentCommand(request.Name, request.Players);
            var result = await startTournamentHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Error!.Message });
            }

            var data = result.Value!;
            return Ok(new
            {
                tournamentId = data.Tournament.Id,
                name = data.Tournament.Name,
                numberOfPlayers = data.Tournament.NumberOfPlayers,
                currentRound = data.Tournament.CurrentRound,
                matches = data.Matches.Select(m => new
                {
                    matchId = m.Id,
                    round = m.Round,
                    player1 = data.Players.First(p => p.Id == m.Player1Id).Name,
                    player2 = data.Players.First(p => p.Id == m.Player2Id).Name
                })
            });
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetTournamentStatus([FromQuery] int tournamentId, [FromQuery] int playerId)
        {
            var query = new GetTournamentStatusQuery(tournamentId, playerId);
            var result = await getTournamentStatusHandler.HandleAsync(query);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "TOURNAMENT_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "PLAYER_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "MATCH_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "OPPONENT_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    _ => BadRequest(new { message = result.Error.Message })
                };
            }

            var data = result.Value!;
            var isPlayer1 = data.IsPlayer1;
            var currentMatch = data.CurrentMatch;

            var playerWins = isPlayer1 ? currentMatch.Player1Wins : currentMatch.Player2Wins;
            var opponentWins = isPlayer1 ? currentMatch.Player2Wins : currentMatch.Player1Wins;
            const int bestOf = 3;

            var otherMatchesCompleted = data.AllMatchesInRound
                .Where(m => m.Id != currentMatch.Id)
                .All(m => m.IsCompleted);

            return Ok(new
            {
                tournamentId = data.Tournament.Id,
                tournamentName = data.Tournament.Name,
                currentRound = data.Tournament.CurrentRound,
                maxRounds = data.Tournament.NumberOfPlayers - 1,
                player = new
                {
                    id = data.Player.Id,
                    name = data.Player.Name
                },
                opponent = new
                {
                    id = data.Opponent.Id,
                    name = data.Opponent.Name
                },
                match = new
                {
                    matchId = currentMatch.Id,
                    round = $"{data.Tournament.CurrentRound} of {data.Tournament.NumberOfPlayers - 1}",
                    playerWins = playerWins,
                    opponentWins = opponentWins,
                    bestOf = bestOf,
                    isCompleted = currentMatch.IsCompleted,
                    winner = currentMatch.WinnerId.HasValue
                        ? (currentMatch.WinnerId == playerId ? data.Player.Name : data.Opponent.Name)
                        : null
                },
                roundStatus = new
                {
                    allMatchesCompleted = data.AllMatchesInRound.All(m => m.IsCompleted),
                    otherMatchesCompleted = otherMatchesCompleted,
                    completedMatches = data.AllMatchesInRound.Count(m => m.IsCompleted),
                    totalMatches = data.AllMatchesInRound.Count
                }
            });
        }

        [HttpPost("play")]
        public async Task<IActionResult> PlayMove([FromBody] PlayMoveDto request)
        {
            var command = new PlayMoveCommand(request.TournamentId, request.PlayerId, request.Move);
            var result = await playMoveHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "TOURNAMENT_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "MATCH_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "PLAYER_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "TOURNAMENT_COMPLETED" => BadRequest(new { message = result.Error.Message }),
                    "MATCH_COMPLETED" => BadRequest(new { message = result.Error.Message }),
                    "INVALID_MOVE" => BadRequest(new { message = result.Error.Message }),
                    _ => BadRequest(new { message = result.Error.Message })
                };
            }

            var data = result.Value!;
            string[] moveNames = { "Rock", "Paper", "Scissors" };

            return Ok(new
            {
                matchId = data.UpdatedMatch.Id,
                playerMove = moveNames[data.PlayerMove],
                opponentMove = moveNames[data.OpponentMove],
                roundResult = data.GameResult == 1 ? "win" : data.GameResult == -1 ? "loss" : "draw",
                playerWins = data.IsPlayer1 ? data.UpdatedMatch.Player1Wins : data.UpdatedMatch.Player2Wins,
                opponentWins = data.IsPlayer1 ? data.UpdatedMatch.Player2Wins : data.UpdatedMatch.Player1Wins,
                isMatchCompleted = data.UpdatedMatch.IsCompleted,
                matchWinner = data.UpdatedMatch.WinnerId.HasValue
                    ? (data.UpdatedMatch.WinnerId == request.PlayerId ? data.Player.Name : data.Opponent.Name)
                    : null
            });
        }

        [HttpPost("advance")]
        public async Task<IActionResult> AdvanceRound([FromBody] AdvanceRoundDto request)
        {
            var command = new AdvanceRoundCommand(request.TournamentId);
            var result = await advanceRoundHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "TOURNAMENT_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "TOURNAMENT_COMPLETED" => BadRequest(new { message = result.Error.Message }),
                    _ => BadRequest(new { message = result.Error.Message })
                };
            }

            var data = result.Value!;

            if (data.IsCompleted)
            {
                return Ok(new
                {
                    message = "Tournament completed!",
                    tournamentId = data.Tournament.Id,
                    isCompleted = true,
                    currentRound = data.Tournament.CurrentRound
                });
            }

            var maxRounds = data.Tournament.NumberOfPlayers - 1;

            return Ok(new
            {
                message = "Round advanced successfully.",
                tournamentId = data.Tournament.Id,
                currentRound = data.Tournament.CurrentRound,
                maxRounds = maxRounds,
                newMatches = data.NewMatches!.Select(m => new
                {
                    matchId = m.Id,
                    player1 = data.Players!.First(p => p.Id == m.Player1Id).Name,
                    player2 = data.Players!.First(p => p.Id == m.Player2Id).Name
                })
            });
        }

        [HttpGet("final")]
        public async Task<IActionResult> GetFinalResults([FromQuery] int tournamentId)
        {
            var query = new GetFinalResultsQuery(tournamentId);
            var result = await getFinalResultsHandler.HandleAsync(query);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "TOURNAMENT_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "TOURNAMENT_NOT_COMPLETED" => BadRequest(new { message = result.Error.Message }),
                    _ => BadRequest(new { message = result.Error.Message })
                };
            }

            var data = result.Value!;
            var winner = data.Scoreboard.FirstOrDefault();

            return Ok(new
            {
                tournamentId = data.Tournament.Id,
                tournamentName = data.Tournament.Name,
                isCompleted = data.Tournament.IsCompleted,
                totalRounds = data.Tournament.NumberOfPlayers - 1,
                winner = winner != null ? new
                {
                    playerId = winner.PlayerId,
                    playerName = winner.PlayerName,
                    wins = winner.Wins,
                    losses = winner.Losses
                } : null,
                scoreboard = data.Scoreboard.Select(s => new
                {
                    rank = s.Rank,
                    playerId = s.PlayerId,
                    playerName = s.PlayerName,
                    wins = s.Wins,
                    losses = s.Losses
                })
            });
        }
    }
}
