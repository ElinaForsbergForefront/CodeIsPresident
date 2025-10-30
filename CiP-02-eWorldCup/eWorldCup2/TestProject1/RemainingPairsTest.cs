using eWorldCup2.Application;

namespace eWorldCup2.Tests;

public class RemainingPairsTest
{
    private RemainingPairsCalculator _calculator = null!;
    [SetUp]
    public void Setup()
    {
        _calculator = new RemainingPairsCalculator();
    }

    [Test]
    public void Should_Return_True_When_Remaining_Pair_Is_True()
    {
        var result = _calculator.CalculateRemainingPairs(10, 3);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(30));
    }
}
