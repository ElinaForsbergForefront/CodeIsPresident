using ConsoleAppHowManyFizzBuzz.Klasser;

var talet = InputReader.ReadPositiveBigInteger("Skriv ett positivt heltal tack:");

var rules = new IFizzBuzzRule[] 
{ 
    new FizzRule(), 
    new BuzzRule() 
};


foreach (var rule in rules)
{
    Console.WriteLine($"Antal {rule.Value}: {rule.Calculation(talet)}");
}




