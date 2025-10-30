using eWorldCup2.Application;

namespace eWorldCup2.Tests;

public class MaxRoundTest
{

    private MaxRoundsCalculator _calculator = null!;
    [SetUp]
    public void Setup()
    {
        _calculator = new MaxRoundsCalculator();
    }

    [Test]
    public void Shold_Be_True_If_Calculator_Is_Correct()
    {
        var result = _calculator.CalculateMaxRounds(10);

        
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(9));
    }


    [Test]
    public void Should_Be_False_If_Number_Of_Players_Is_Odd()
    {
        var result = _calculator.CalculateMaxRounds(7);

        Assert.That(result.IsSuccess, Is.False, "Borde returnera failure");
        Assert.That(result.Error.Code, Is.EqualTo("INVALID_INPUT"));
    }
}
