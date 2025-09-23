using System.Numerics;
using ConsoleAppHowManyFizzBuzz.Klasser;

namespace FizzBuzz.tester;

public class BuzzRuleTest
{
    private BuzzRule _rule;

    [SetUp]
    public void SetUp() => _rule = new BuzzRule();

    [Test]
    public void WhenNumberIs9_Calculation_Returns1()
    {
        Assert.AreEqual(1, (int)_rule.Calculation(9));
    }

    [Test]
    public void WhenNumberIs15_Calculation_Returns3()
    {
        Assert.AreEqual(3, (int)_rule.Calculation(15)); 
    }

    [Test]
    public void WhenNumberIsLarge_Calculation_ReturnsExpected()
    {
        BigInteger n = 1_000;
        var result = _rule.Calculation(n);
        Assert.AreEqual(200, (int)result);
    }

}
