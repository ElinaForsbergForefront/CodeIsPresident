using System;
using System.Collections.Generic;
using System.Text;
using eWorldCup2.Domain.ROP;

namespace eWorldCup2.Application.Queries.GetMaxRounds
{
    public class GetMaxRoundsQueryHandler
    {
        private readonly MaxRoundsCalculator maxRoundsCalculator;

        public GetMaxRoundsQueryHandler(MaxRoundsCalculator maxRoundsCalculator)
        {
            this.maxRoundsCalculator = maxRoundsCalculator;
        }

        public Result<GetMaxRoundsResult> Handle(GetMaxRoundsQuery query)
        {
            var result = maxRoundsCalculator.CalculateMaxRounds(query.NumberOfPlayers);

            if (!result.IsSuccess)
            {
                return Result<GetMaxRoundsResult>.Failure(result.Error!);
            }

            var queryResult = new GetMaxRoundsResult(result.Value);
            return Result<GetMaxRoundsResult>.Success(queryResult);
        }
    }
}
