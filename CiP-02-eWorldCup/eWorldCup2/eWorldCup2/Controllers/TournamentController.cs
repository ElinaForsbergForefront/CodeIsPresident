
using eWorldCup2.Api.Dtos;
using eWorldCup2.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace eWorldCup2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly TournamentService tournamentService;

        public TournamentController(TournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartTournament([FromBody] StartTournamentDto request)
        {
            var result = await tournamentService.StartTournamentAsync(request.Name, request.Players);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.Error!.Message });
            }

            var (tournament, matches, players) = result.Value!;

            return Ok(new
            {
                tournamentId = tournament.Id,
                name = tournament.Name,
                numberOfPlayers = tournament.NumberOfPlayers,
                currentRound = tournament.CurrentRound,
                matches = matches.Select(m => new
                {
                    matchId = m.Id,
                    round = m.Round,
                    player1 = players.First(p => p.Id == m.Player1Id).Name,
                    player2 = players.First(p => p.Id == m.Player2Id).Name
                })
            });
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetTournamentStatus([FromQuery] int tournamentId, [FromQuery] int playerId)
        {
            var result = await tournamentService.GetTournamentStatusAsync(tournamentId, playerId);

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

            var (tournament, currentMatch, player, opponent, isPlayer1, allMatchesInRound) = result.Value!;

            var playerWins = isPlayer1 ? currentMatch.Player1Wins : currentMatch.Player2Wins;
            var opponentWins = isPlayer1 ? currentMatch.Player2Wins : currentMatch.Player1Wins;
            const int bestOf = 3;

            var otherMatchesCompleted = allMatchesInRound
                .Where(m => m.Id != currentMatch.Id)
                .All(m => m.IsCompleted);

            return Ok(new
            {
                tournamentId = tournament.Id,
                tournamentName = tournament.Name,
                currentRound = tournament.CurrentRound,
                maxRounds = tournament.NumberOfPlayers - 1,
                player = new
                {
                    id = player.Id,
                    name = player.Name
                },
                opponent = new
                {
                    id = opponent.Id,
                    name = opponent.Name
                },
                match = new
                {
                    matchId = currentMatch.Id,
                    round = $"{tournament.CurrentRound} of {tournament.NumberOfPlayers - 1}",
                    playerWins = playerWins,
                    opponentWins = opponentWins,
                    bestOf = bestOf,
                    isCompleted = currentMatch.IsCompleted,
                    winner = currentMatch.WinnerId.HasValue
                        ? (currentMatch.WinnerId == playerId ? player.Name : opponent.Name)
                        : null
                },
                roundStatus = new
                {
                    allMatchesCompleted = allMatchesInRound.All(m => m.IsCompleted),
                    otherMatchesCompleted = otherMatchesCompleted,
                    completedMatches = allMatchesInRound.Count(m => m.IsCompleted),
                    totalMatches = allMatchesInRound.Count
                }
            });
        }

        [HttpPost("play")]
        public async Task<IActionResult> PlayMove([FromBody] PlayMoveDto request)
        {
            var result = await tournamentService.PlayMoveAsync(request.TournamentId, request.PlayerId, request.Move);

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

            var (updatedMatch, player, opponent, isPlayer1, playerMove, opponentMove, gameResult) = result.Value!;

            // Convert move integers to strings
            string[] moveNames = { "Rock", "Paper", "Scissors" };

            return Ok(new
            {
                matchId = updatedMatch.Id,
                playerMove = moveNames[playerMove],
                opponentMove = moveNames[opponentMove],
                roundResult = gameResult == 1 ? "win" : gameResult == -1 ? "loss" : "draw",
                playerWins = isPlayer1 ? updatedMatch.Player1Wins : updatedMatch.Player2Wins,
                opponentWins = isPlayer1 ? updatedMatch.Player2Wins : updatedMatch.Player1Wins,
                isMatchCompleted = updatedMatch.IsCompleted,
                matchWinner = updatedMatch.WinnerId.HasValue
                    ? (updatedMatch.WinnerId == request.PlayerId ? player.Name : opponent.Name)
                    : null
            });
        }

        [HttpPost("advance")]
        public async Task<IActionResult> AdvanceRound([FromBody] AdvanceRoundDto request)
        {
            var result = await tournamentService.AdvanceRoundAsync(request.TournamentId);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "TOURNAMENT_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "TOURNAMENT_COMPLETED" => BadRequest(new { message = result.Error.Message }),
                    _ => BadRequest(new { message = result.Error.Message })
                };
            }

            var (tournament, newMatches, players, isCompleted) = result.Value!;

            if (isCompleted)
            {
                return Ok(new
                {
                    message = "Tournament completed!",
                    tournamentId = tournament.Id,
                    isCompleted = true,
                    currentRound = tournament.CurrentRound
                });
            }

            var maxRounds = tournament.NumberOfPlayers - 1;

            return Ok(new
            {
                message = "Round advanced successfully.",
                tournamentId = tournament.Id,
                currentRound = tournament.CurrentRound,
                maxRounds = maxRounds,
                newMatches = newMatches!.Select(m => new
                {
                    matchId = m.Id,
                    player1 = players!.First(p => p.Id == m.Player1Id).Name,
                    player2 = players!.First(p => p.Id == m.Player2Id).Name
                })
            });
        }

        [HttpGet("final")]
        public async Task<IActionResult> GetFinalResults([FromQuery] int tournamentId)
        {
            var result = await tournamentService.GetFinalResultsAsync(tournamentId);

            if (!result.IsSuccess)
            {
                return result.Error!.Code switch
                {
                    "TOURNAMENT_NOT_FOUND" => NotFound(new { message = result.Error.Message }),
                    "TOURNAMENT_NOT_COMPLETED" => BadRequest(new { message = result.Error.Message }),
                    _ => BadRequest(new { message = result.Error.Message })
                };
            }

            var (tournament, scoreboard) = result.Value!;
            var winner = scoreboard.FirstOrDefault();

            return Ok(new
            {
                tournamentId = tournament.Id,
                tournamentName = tournament.Name,
                isCompleted = tournament.IsCompleted,
                totalRounds = tournament.NumberOfPlayers - 1,
                winner = winner.Rank > 0 ? new
                {
                    playerId = winner.PlayerId,
                    playerName = winner.PlayerName,
                    wins = winner.Wins,
                    losses = winner.Losses
                } : null,
                scoreboard = scoreboard.Select(s => new
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
