using eWorldCup2.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace eWorldCup2.Api.Controllers
{
    public class TournamentController : Controller
    {
        private readonly WorldCupDbContext dbContext;

        public TournamentController(WorldCupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("tournament/start")]
        public IActionResult StartTournament()
        {
            ///
            //Startar ny turnering.Skapar par för runda 1 baserat
            //på din round-robin - funktion.Body: 
            //{ "name": "Alice", "players": 8 }.
            // Placeholder for starting tournament logic
            return Ok(new { Message = "Tournament started!" });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
