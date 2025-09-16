public interface IFizzBuzzRule
{
    bool AppliesTo(int number);
    string GetValue();
}

public class FizzRule : IFizzBuzzRule
{
    public bool AppliesTo(int number) 
    { 
        if (number % 3 == 0) return true; 
        return false; 
    }
    public string GetValue() => "Fizz";
}

public class BuzzRule : IFizzBuzzRule
{
    public bool AppliesTo(int number) 
    { 
        if (number % 5 == 0) return true; 
        return false; 
    }
    public string GetValue() => "Buzz";
}

public class FizzBuzzRule : IFizzBuzzRule
{
    public bool AppliesTo(int number)
    {
        if (number % 3 == 0 && number % 5 == 0) return true;
        return false;
    }

    public string GetValue() => "Fizzbuzz";
}

public class FizzBuzzCalculator
{
    private readonly IEnumerable<IFizzBuzzRule> _rules;
    public FizzBuzzCalculator(IEnumerable<IFizzBuzzRule> rules) => _rules = rules;

    public string Calculate(int number)
    {
        var result = _rules.FirstOrDefault(r => r.AppliesTo(number));

        return result != null ? result.GetValue() : number.ToString();
    }
}

class Program
{
    static void Main()
    {
        var rules = new List<IFizzBuzzRule>
        {             
            new FizzBuzzRule(),
            new FizzRule(), 
            new BuzzRule(),
        };
        var calculator = new FizzBuzzCalculator(rules);

        for (int i = 1; i <= 100; i++)
            Console.WriteLine(calculator.Calculate(i));
    }
}