using System;
using System.Collections.Generic;
using eWorldCup1.Interfaces;
using eWorldCup1.Models;
using eWorldCup1.Services;
using eWorldCup1.Scheduling;
using eWorldCup1.Repositories;  


class Program
{
    static void Main(string[] args)
    {
        // Dependency injection setup
        var deltagarRepository = new DeltagareRepository();
        var scheduler = new RoundRobinScheduler();
        var inputService = new ConsoleInputService();
        var outputService = new ConsoleOutputService();

        var tournamentService = new TournamentService(
            deltagarRepository,
            scheduler,
            inputService,
            outputService);

        tournamentService.RunTurnering();
    }
}