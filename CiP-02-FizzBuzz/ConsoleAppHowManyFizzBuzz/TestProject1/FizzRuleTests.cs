using System.Numerics;
using ConsoleAppHowManyFizzBuzz.Klasser;

namespace FizzBuzz.tester;

public class FizzRuleTests
{
    private FizzRule _rule;

    [SetUp]
    public void SetUp() => _rule = new FizzRule();

    [Test]
    public void WhenNumberIs15_Calculation_Returns5()
    {
        var result = _rule.Calculation(15); 
        Assert.AreEqual(5, (int)result);
    }

    [Test]
    public void WhenNumberIsLarge_Calculation_ReturnsExpected()
    {
        BigInteger n = 1_000;
        var result = _rule.Calculation(n);
        Assert.AreEqual(333, (int)result);
    }
}
