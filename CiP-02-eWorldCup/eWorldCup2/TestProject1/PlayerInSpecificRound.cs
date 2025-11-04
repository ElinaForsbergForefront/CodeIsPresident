using eWorldCup2.Application;
using eWorldCup2.Domain.Models;

namespace eWorldCup2.Tests;

public class PlayerInSpecificRound
{
    private SpecifikPlayerRound _specificRound = null!;
    [SetUp]
    public void Setup()
    {
        _specificRound = new SpecifikPlayerRound();
    }

    [Test]
    public void Should_Return_Ethan_Fiona_When_Id_Is_4()
    {

        var players = new List<Player>
        {
            new(1, "Alice"),
            new(2, "Bob"),
            new(3, "Charlie"),
            new(4, "Diana"),
            new(5, "Ethan"),
            new(6, "Fiona"),
            new(7, "George"),
            new(8, "Hannah"),
            new(9, "Isaac"),
            new(10, "Julia"),
        };

        var result = _specificRound.GetSpecificRound(players, 10, 4, 2); 

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo((players[4], players[5])));
    }
}
