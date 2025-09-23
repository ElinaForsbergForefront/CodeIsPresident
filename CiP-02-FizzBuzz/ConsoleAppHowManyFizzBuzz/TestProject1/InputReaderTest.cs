using System.Numerics;
using ConsoleAppHowManyFizzBuzz.Klasser;

namespace FizzBuzz.tester;

public class InputReaderTest
{
    private TextReader _origIn = null!;
    private TextWriter _origOut = null!;

    [SetUp]
    public void SetUp()
    {
        _origIn = Console.In;
        _origOut = Console.Out;
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetIn(_origIn);
        Console.SetOut(_origOut);
    }

    [Test]
    public void WhenTooLarge_ShowsErrorMessage()
    {
        var tooLarge = (InputReader.MaxValue + 1).ToString();
        var valid = "1";
        var input = tooLarge + Environment.NewLine + valid + Environment.NewLine;
        Console.SetIn(new StringReader(input));
        var writer = new StringWriter();
        Console.SetOut(writer);

        InputReader.ReadPositiveBigInteger("Gränstest:");

        var output = writer.ToString();
        StringAssert.Contains("Fel.", output);
        StringAssert.Contains(InputReader.MaxValue.ToString(), output);
    }

    [Test]
    public void WhenInputIsMinimum_ReturnsOne()
    {
        var input = "1" + Environment.NewLine;
        Console.SetIn(new StringReader(input));
        var writer = new StringWriter();
        Console.SetOut(writer);

        var value = InputReader.ReadPositiveBigInteger("Min test:");

        Assert.AreEqual(1, (int)value);
    }


}
