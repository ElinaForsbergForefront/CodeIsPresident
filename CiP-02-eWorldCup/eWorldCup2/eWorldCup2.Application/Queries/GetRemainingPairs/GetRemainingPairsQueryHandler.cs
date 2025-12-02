using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;

namespace eWorldCup2.Application.Queries.GetRemainingPairs
{
    public class GetRemainingPairsQueryHandler
    {
        private readonly RemainingPairsCalculator remainingPairsCalculator;

        public GetRemainingPairsQueryHandler(RemainingPairsCalculator remainingPairsCalculator)
        {
            this.remainingPairsCalculator = remainingPairsCalculator;
        }

        public Result<GetRemainingPairsResult> Handle(GetRemainingPairsQuery query)
        {
            var result = remainingPairsCalculator.CalculateRemainingPairs(
                query.NumberOfPlayers,
                query.CompletedRounds);

            if (!result.IsSuccess)
            {
                return Result<GetRemainingPairsResult>.Failure(result.Error!);
            }

            var queryResult = new GetRemainingPairsResult(result.Value);
            return Result<GetRemainingPairsResult>.Success(queryResult);
        }
    }
}
