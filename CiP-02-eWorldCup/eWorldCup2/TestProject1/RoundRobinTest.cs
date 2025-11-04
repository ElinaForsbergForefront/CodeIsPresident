using System.Reflection.Metadata;
using System.Xml.Linq;
using eWorldCup2.Application;
using eWorldCup2.Domain.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RoundRobinTest
{
    public class Tests
    {
        private RoundRobinService _service = null!;

        [SetUp]
        public void Setup()
        {
            _service = new RoundRobinService();
        }
        [Test]
        public void Should_Generate_Correct_Number_Of_Pairs()
        {
            var players = new List<Player>
            {
                new(1, "Alice"), 
                new(2, "Bob"), 
                new(3,"Charlie"), 
                new(4, "Diana"), 
                new(5, "Ethan"), 
                new(6, "Fiona")
            };
            
            var result = _service.GetRoundPairs(players, 2);

            Assert.That(result.IsSuccess, Is.True, "Borde returnera success");
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value!.Count, Is.EqualTo(3)); // 6 spelare → 3 par
            Assert.That(result.Value, Is.EqualTo(new List<(Player, Player)>
            {
                (players[0], players[5]), // Alice vs Fiona
                (players[1], players[4]), // Bob vs Ethan
                (players[2], players[3])  // Charlie vs Diana
            }));
            Assert.That(result.Value.All(p => p.Item1.Id != p.Item2.Id), Is.True);
        }
    }
}